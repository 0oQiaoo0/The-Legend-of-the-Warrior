using Player;
using UnityEngine;

namespace General
{
    public class PhysicsCheck : MonoBehaviour
    {
        private CapsuleCollider2D _capsuleCollider;
        private BoxCollider2D _boxCollider;
        private PlayerController _playerController;
        private Rigidbody2D _rigidbody;
        [Header("检测参数")]
        public bool manual;
        public Vector2 bottomOffset;
        public Vector2 leftBottomOffset;
        public Vector2 rightBottomOffset;
        public Vector2 leftOffset;
        public Vector2 rightOffset;
        public float checkRadius;
        public LayerMask groundLayer;
        [Header("状态")]
        public bool isGround;
        public bool leftIsGround;
        public bool rightIsGround;
        public bool touchLeftWall;
        public bool touchRightWall;
        public bool onWall;
        private void Awake()
        {
        
            _capsuleCollider = GetComponent<CapsuleCollider2D>() ?? null;
            _boxCollider = GetComponent<BoxCollider2D>() ?? null;
            if(gameObject.layer == 7)//isPlayer
            {
                _rigidbody = GetComponent<Rigidbody2D>();
                _playerController = GetComponent<PlayerController>();
            }
            if (!manual)
            {
                ResetOffset();
            }
            Check();
        }
        public void ResetOffset()
        {
            //if(boxCollider)
            {
                bottomOffset = new Vector2(_boxCollider.offset.x, _boxCollider.offset.y - _boxCollider.size.y / 2);
                leftBottomOffset = new Vector2(_boxCollider.offset.x - _boxCollider.size.x / 2, _boxCollider.offset.y - _boxCollider.size.y / 2);
                rightBottomOffset = new Vector2(_boxCollider.offset.x + _boxCollider.size.x / 2, _boxCollider.offset.y - _boxCollider.size.y / 2);
            }
            //if(capsuleCollider)
            {
                rightOffset = new Vector2(_capsuleCollider.offset.x + _capsuleCollider.bounds.size.x / 2, _capsuleCollider.offset.y);
                leftOffset = new Vector2(_capsuleCollider.offset.x - _capsuleCollider.bounds.size.x / 2, _capsuleCollider.offset.y);
            }
        }
        private void Update()
        {
            Check();
        }

        private void Check()
        {
            //检测地面
            //if (boxCollider)
            {
                isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer);
                leftIsGround = Physics2D.OverlapCircle((Vector2)transform.position + leftBottomOffset, checkRadius, groundLayer);
                rightIsGround = Physics2D.OverlapCircle((Vector2)transform.position + rightBottomOffset, checkRadius, groundLayer);
            }
            //墙体判断
            //if (capsuleCollider)
            {
                touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
                touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);
            }
            if(_playerController)
            {
                onWall = !isGround && _rigidbody.velocity.y < 0.01f && (touchLeftWall && _playerController.inputDirection.x < 0f || touchRightWall && _playerController.inputDirection.x > 0f);
            }
        }

        public void ChangeDirection()
        {
            float tmp;
            //if (boxCollider)
            {
                //bottomOffset翻转
                bottomOffset.x = -bottomOffset.x;
                //leftBottomOffset & rightBottomOffset翻转
                tmp = leftBottomOffset.x;
                leftBottomOffset.x = -rightBottomOffset.x;
                rightBottomOffset.x = -tmp;
            }
            //if (capsuleCollider)
            {
                //leftOffset & rightOffset翻转
                tmp = leftOffset.x;
                leftOffset.x = -rightOffset.x;
                rightOffset.x = -tmp;
            }
        
        }

        private void OnDrawGizmosSelected()
        {
            //if (boxCollider)
            {
                Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
                Gizmos.DrawWireSphere((Vector2)transform.position + leftBottomOffset, checkRadius);
                Gizmos.DrawWireSphere((Vector2)transform.position + rightBottomOffset, checkRadius);
            }
            //if (capsuleCollider)
            {
                Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
                Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
            }
        }
    }
}
