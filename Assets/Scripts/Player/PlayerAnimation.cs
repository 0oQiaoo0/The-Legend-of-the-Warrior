using General;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private PlayerController _playerController;
        private PhysicsCheck _physicsCheck;
        // Start is called before the first frame update
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerController = GetComponent<PlayerController>();
            _physicsCheck = GetComponent<PhysicsCheck>();
        }
        private void Update()
        {
            SetAnimation();
        }

        private void SetAnimation()
        {
            _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody.velocity.x));
            _animator.SetFloat("velocityY", _rigidbody.velocity.y);
            _animator.SetBool("isGround", _playerController.IsGround());
            _animator.SetBool("isCrouch", _playerController.isCrouch);
            _animator.SetBool("isDead", _playerController.isDead);
            _animator.SetBool("isAttack", _playerController.isAttack);
            _animator.SetBool("onWall", _physicsCheck.onWall);
            _animator.SetBool("isSlide", _playerController.isSlide);
        }

        public void PlayHurt()
        {
            _animator.SetTrigger("hurt");
        }

        public void PlayAttack()
        {
            _animator.SetTrigger("attack");
        }
    }
}
