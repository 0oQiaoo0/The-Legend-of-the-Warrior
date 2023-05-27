using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    public Vector2 inputDirection;
    [Header("基本参数")]
    public float speed;
    private float runSpeed;
    private float walkSpeed;
    public float jumpForce;
    public float hurtForce;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInoutControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;

        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.CrouchButton.started += CrouchStart;
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
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt&&!isAttack&&!isCrouch)
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
    private void Jump(InputAction.CallbackContext obj)
    {
        if(IsGround())
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void CrouchStart(InputAction.CallbackContext obj)
    {
        if (IsGround())
        {
            isCrouch = true;
            capsuleCollider.offset = new Vector2(-0.05f, 0.85f);
            capsuleCollider.size = new Vector2(0.7f, 1.7f);
        }
    }
    private void CrouchEnd(InputAction.CallbackContext obj)
    {
        isCrouch = false;
        capsuleCollider.size = originalSize;
        capsuleCollider.offset = originalOffset;
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
        boxCollider.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
}
