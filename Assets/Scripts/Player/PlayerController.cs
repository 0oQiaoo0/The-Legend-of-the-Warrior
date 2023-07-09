using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Serialization;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class PlayerController : MonoBehaviour
{
    private PhysicsCheck physicsCheck;
    public PlayerInoutControl inputControl;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D boxCollider;
    private PlayerAnimation playerAnimation;
    private Character character;
    public UIManager uiManager;
    public Vector2 inputDirection;
    public AudioDefination jumpAudioDefination;
    [Header("监听事件")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    [Header("基本参数")]
    public float speed;
    private float runSpeed;
    private float walkSpeed;
    public float jumpForce;
    public float wallJumpForce;
    public float wallJumpTime;
    public Vector2 leftWallForceAngle;
    public Vector2 rightWallForceAngle;
    public float wallJumpPowerCost;
    public float hurtForce;

    public float slideDistance;
    public float slideFrameSpeed;
    public float slidePowerCost;
    private Coroutine slideCouroutine;

    private Vector2 originalOffset;
    private Vector2 originalSize;

    public float lastDirTime;
    private float lastLeftTimeCounter;
    private float lastRightTimeCounter;
    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    public PhysicsMaterial2D OnWall;
    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool isWallJump;
    public bool isSlide;


    private void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInoutControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;

        inputControl.Gameplay.Jump.started += JumporWallJump;
        inputControl.Gameplay.CrouchButton.started += CrouchorSlideStart;
        inputControl.Gameplay.CrouchButton.canceled += CrouchEnd;

        #region 强制走路
        runSpeed = speed;
        walkSpeed = speed / 2.5f;
        
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (IsGround())
            {
                speed = walkSpeed;
            }
        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (IsGround())
            {
                speed = runSpeed;
            }
        };
        #endregion

        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }

    

    private void OnEnable()
    {
        inputControl.Enable();
        loadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        loadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }
    private void OnAfterSceneLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt&&!isAttack&&!isCrouch&&!isWallJump&&!isSlide)
            Move();
    }

    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0) 
            faceDir = -1;
        //人物翻转
        if(faceDir != (int)transform.localScale.x)
            ChangeDirection();
    }

    public void ChangeDirection()
    {
        //change scale
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        physicsCheck.ChangeDirection();
    }

    #region UnityEvent
    private void JumporWallJump(InputAction.CallbackContext obj)
    {
        if (IsGround())
        {
            if (isSlide)
            {
                SlideEnd();
                StopCoroutine(slideCouroutine);
            }
            Jump();
        }
        if (physicsCheck.onWall && rb.velocity.y < 0.01f)
        {
            if (character.currentPower >= wallJumpPowerCost)
            {
                WallJump();
            }
            else
            {
                uiManager.PowerLack();
            }
                
        }
    }
    private void Jump()
    {
        jumpAudioDefination.PlayAudioClip();

        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void WallJump()
    {
        Vector2 forceAngle = Vector2.zero;
        if (physicsCheck.touchLeftWall && lastLeftTimeCounter > 0) forceAngle = leftWallForceAngle.normalized;
        if (physicsCheck.touchRightWall && lastRightTimeCounter > 0) forceAngle = rightWallForceAngle.normalized;

        jumpAudioDefination.PlayAudioClip();

        rb.velocity = Vector2.zero;
        rb.AddForce(forceAngle * wallJumpForce, ForceMode2D.Impulse);
        isWallJump = true;
        character.PowerSpend(wallJumpPowerCost);
    }
    private void CrouchorSlideStart(InputAction.CallbackContext obj)
    {
        if (IsGround())
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                isCrouch = true;
                capsuleCollider.offset = new Vector2(-0.05f, 0.85f);
                capsuleCollider.size = new Vector2(0.7f, 1.7f);
                physicsCheck.resetOffset();
            }
            else
            {
                if(!isSlide)
                {
                    if(character.currentPower >= slidePowerCost) 
                    {
                        SlideBegin();
                    }
                    else
                    {
                        uiManager.PowerLack();
                    }
                }
            }
        }
    }
    private void SlideBegin()
    {
        isSlide = true;
        gameObject.layer = 2;//ignore trigger
        var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);
        slideCouroutine = StartCoroutine(TriggerSlide(targetPos));
        character.PowerSpend(slidePowerCost);
    }
    private IEnumerator TriggerSlide(Vector3 target)
    {
        //rb.MovePosition(target);
        while (MathF.Abs(target.x - transform.position.x) > 0.2f)
        {
            //Debug.Log(MathF.Abs(target.x - transform.position.x));
            if (!FrontIsGround() || TouchFrontWall()) 
            {
                SlideEnd();
                yield break;
            }

            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideFrameSpeed, transform.position.y));

            yield return null;
        }
        //Debug.Log("slide end");
        SlideEnd();
    }
    private bool TouchFrontWall()
    {
        return physicsCheck.touchLeftWall && transform.localScale.x == -1 || physicsCheck.touchRightWall && transform.localScale.x == 1;
    }
    private void SlideEnd()
    {
        isSlide = false;
        gameObject.layer = 7;//player
    }
    private void CrouchEnd(InputAction.CallbackContext obj)
    {
        if (isCrouch)
        {
            isCrouch = false;
            capsuleCollider.size = originalSize;
            capsuleCollider.offset = originalOffset;
            physicsCheck.resetOffset();
        }
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if(IsGround())
        {
            playerAnimation.PlayAttack();
            isAttack = true;
        }
    }
    #endregion

    public bool IsGround()
    {
        if(transform.localScale.x == 1)
        {
            return physicsCheck.isGround || physicsCheck.leftIsGround;
        }
        else
        {
            return physicsCheck.isGround || physicsCheck.rightIsGround;
        }
        
    }

    public bool FrontIsGround()
    {
        return transform.localScale.x == 1 && physicsCheck.rightIsGround || transform.localScale.x == -1 && physicsCheck.leftIsGround;
    }
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        gameObject.layer = 2;
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    private void CheckState()
    {
        boxCollider.sharedMaterial = IsGround() ? normal : physicsCheck.onWall ? OnWall : wall;
        capsuleCollider.sharedMaterial = IsGround() ? normal : physicsCheck.onWall ? OnWall : wall;

        if (isWallJump) StartCoroutine(WallJumpCounter());

        if (lastLeftTimeCounter > -1) lastLeftTimeCounter -= Time.deltaTime;
        if (lastRightTimeCounter > -1) lastRightTimeCounter -= Time.deltaTime;

        if (inputDirection.x > 0) lastRightTimeCounter = lastDirTime;
        if (inputDirection.x < 0) lastLeftTimeCounter = lastDirTime;
    }

    IEnumerator WallJumpCounter()
    {
        yield return new WaitForSeconds(wallJumpTime);
        isWallJump = false;
    }
}
