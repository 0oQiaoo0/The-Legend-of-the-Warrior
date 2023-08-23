using System;
using System.Collections;
using Audio;
using General;
using SO;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PhysicsCheck _physicsCheck;
        private PlayerInoutControl _inputControl;
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _capsuleCollider;
        private BoxCollider2D _boxCollider;
        private PlayerAnimation _playerAnimation;
        private Character _character;
        public UIManager uiManager;
        public Vector2 inputDirection;
        public AudioDefinition jumpAudioDefinition;
        [Header("监听事件")]
        public SceneLoadEventSO sceneLoadEvent;
        public VoidEventSO afterSceneLoadedEvent;
        public VoidEventSO loadGameEvent;
        public VoidEventSO backToMenuEvent;
        [Header("基本参数")]
        public float speed;
        public float runSpeed = 290;
        public float walkSpeed = 116;
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
        private Coroutine _slideCoroutine;

        private Vector2 _originalOffset;
        private Vector2 _originalSize;

        public float lastDirTime;
        private float _lastLeftTimeCounter;
        private float _lastRightTimeCounter;
        [Header("物理材质")]
        public PhysicsMaterial2D normal;
        public PhysicsMaterial2D wall;
        public PhysicsMaterial2D onWall;
        [Header("状态")]
        public bool isCrouch;
        public bool isHurt;
        public bool isDead;
        public bool isAttack;
        public bool isWallJump;
        public bool isSlide;


        private void Awake()
        {
            _character = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _inputControl = new PlayerInoutControl();
            _physicsCheck = GetComponent<PhysicsCheck>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _playerAnimation = GetComponent<PlayerAnimation>();
            _originalOffset = _capsuleCollider.offset;
            _originalSize = _capsuleCollider.size;

            _inputControl.Gameplay.Jump.started += JumporWallJump;
            _inputControl.Gameplay.CrouchButton.started += CrouchSlideStart;
            _inputControl.Gameplay.CrouchButton.canceled += CrouchEnd;

            #region 强制走路
            // runSpeed = speed;
            // walkSpeed = speed / 2.5f;
        
            _inputControl.Gameplay.WalkButton.performed += ctx =>
            {
                if (IsGround())
                {
                    speed = walkSpeed;
                }
            };
            _inputControl.Gameplay.WalkButton.canceled += ctx =>
            {
                if (IsGround())
                {
                    speed = runSpeed;
                }
            };
            #endregion

            //攻击
            _inputControl.Gameplay.Attack.started += PlayerAttack;
            _inputControl.Enable();
        }

        private void OnEnable()
        {
            sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
            afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
            loadGameEvent.OnEventRaised += OnLoadDataEvent;
            backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        }

        private void OnDisable()
        {
            // _inputControl.Disable();
            sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
            afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
            loadGameEvent.OnEventRaised -= OnLoadDataEvent;
            backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        }

        private void OnLoadDataEvent()
        {
            isDead = false;
        }

        private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
        {
            _inputControl.Gameplay.Disable();
        }
        private void OnAfterSceneLoadedEvent()
        {
            _inputControl.Gameplay.Enable();
        }

        private void Update()
        {
            inputDirection = _inputControl.Gameplay.Move.ReadValue<Vector2>();
            CheckState();
        }

        private void FixedUpdate()
        {
            if (!isHurt&&!isAttack&&!isCrouch&&!isWallJump&&!isSlide)
                Move();
        }

        private void Move()
        {
            _rigidbody.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, _rigidbody.velocity.y);

            int faceDir = (int)transform.localScale.x;

            if (inputDirection.x > 0)
                faceDir = 1;
            if (inputDirection.x < 0) 
                faceDir = -1;
            //人物翻转
            if(faceDir != (int)transform.localScale.x)
                ChangeDirection();
        }

        private void ChangeDirection()
        {
            //change scale
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _physicsCheck.ChangeDirection();
        }

        #region UnityEvent
        private void JumporWallJump(InputAction.CallbackContext obj)
        {
            if (IsGround())
            {
                if (isSlide)
                {
                    SlideEnd();
                    StopCoroutine(_slideCoroutine);
                }
                Jump();
            }
            if (_physicsCheck.onWall && _rigidbody.velocity.y < 0.01f)
            {
                if (_character.currentPower >= wallJumpPowerCost)
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
            jumpAudioDefinition.PlayAudioClip();

            _rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        private void WallJump()
        {
            var forceAngle = Vector2.zero;
            if (_physicsCheck.touchLeftWall && _lastLeftTimeCounter > 0) forceAngle = leftWallForceAngle.normalized;
            if (_physicsCheck.touchRightWall && _lastRightTimeCounter > 0) forceAngle = rightWallForceAngle.normalized;

            jumpAudioDefinition.PlayAudioClip();

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(forceAngle * wallJumpForce, ForceMode2D.Impulse);
            isWallJump = true;
            _character.PowerSpend(wallJumpPowerCost);
        }
        private void CrouchSlideStart(InputAction.CallbackContext obj)
        {
            if (IsGround())
            {
                if (Mathf.Abs(_rigidbody.velocity.x) < 0.1f)
                {
                    isCrouch = true;
                    _capsuleCollider.offset = new Vector2(-0.05f, 0.85f);
                    _capsuleCollider.size = new Vector2(0.7f, 1.7f);
                    _physicsCheck.ResetOffset();
                }
                else
                {
                    if (isSlide) return;
                    
                    if(_character.currentPower >= slidePowerCost) 
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
        private void SlideBegin()
        {
            isSlide = true;
            gameObject.layer = 2;//ignore trigger
            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);
            _slideCoroutine = StartCoroutine(TriggerSlide(targetPos));
            _character.PowerSpend(slidePowerCost);
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

                _rigidbody.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideFrameSpeed, transform.position.y));

                yield return null;
            }
            //Debug.Log("slide end");
            SlideEnd();
        }
        private bool TouchFrontWall()
        {
            return _physicsCheck.touchLeftWall && transform.localScale.x == -1 || _physicsCheck.touchRightWall && transform.localScale.x == 1;
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
                _capsuleCollider.size = _originalSize;
                _capsuleCollider.offset = _originalOffset;
                _physicsCheck.ResetOffset();
            }
        }

        private void PlayerAttack(InputAction.CallbackContext obj)
        {
            if (!IsGround()) return;
            
            _playerAnimation.PlayAttack();
            isAttack = true;
        }
        #endregion

        public bool IsGround()
        {
            if(transform.localScale.x == 1)
            {
                return _physicsCheck.isGround || _physicsCheck.leftIsGround;
            }
            else
            {
                return _physicsCheck.isGround || _physicsCheck.rightIsGround;
            }
        
        }

        private bool FrontIsGround()
        {
            return transform.localScale.x == 1 && _physicsCheck.rightIsGround || transform.localScale.x == -1 && _physicsCheck.leftIsGround;
        }
        public void GetHurt(Transform attacker)
        {
            isHurt = true;
            _rigidbody.velocity = Vector2.zero;
            Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
            _rigidbody.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        }

        public void PlayerDead()
        {
            gameObject.layer = 2;
            isDead = true;
            _inputControl.Gameplay.Disable();
        }
        private void CheckState()
        {
            _boxCollider.sharedMaterial = IsGround() ? normal : _physicsCheck.onWall ? onWall : wall;
            _capsuleCollider.sharedMaterial = IsGround() ? normal : _physicsCheck.onWall ? onWall : wall;

            if (isWallJump) StartCoroutine(WallJumpCounter());

            if (_lastLeftTimeCounter > -1) _lastLeftTimeCounter -= Time.deltaTime;
            if (_lastRightTimeCounter > -1) _lastRightTimeCounter -= Time.deltaTime;

            if (inputDirection.x > 0) _lastRightTimeCounter = lastDirTime;
            if (inputDirection.x < 0) _lastLeftTimeCounter = lastDirTime;
        }

        private IEnumerator WallJumpCounter()
        {
            yield return new WaitForSeconds(wallJumpTime);
            isWallJump = false;
        }
    }
}
