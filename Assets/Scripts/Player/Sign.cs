using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using Utilities;

namespace Player
{
    public class Sign : MonoBehaviour
    {
        private PlayerInoutControl _playerInput;
        private Animator _animator;
        public Transform playerTrans;
        public GameObject signSprite;

        private IInteractable _targetItem;
        private bool _canPress;

        private InputDevice _inputDevice;

        private void Awake()
        {
            //animator = GetComponentInChildren<Animator>();
            _animator = signSprite.GetComponent<Animator>();
            _playerInput = new PlayerInoutControl();
            _playerInput.Enable();

            _inputDevice = new InputDevice();
        }

        private void OnEnable()
        {
            InputSystem.onActionChange += OnActionChange;
            _playerInput.Gameplay.Interact.started += OnConfirm;
        }

        private void OnConfirm(InputAction.CallbackContext context)
        {
            if (_canPress)
            {
                _targetItem.TriggerAction();
            }
        }

        /// <summary>
        /// 切换设备同时切换按钮动画
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="actionChange"></param>
        private void OnActionChange(object obj, InputActionChange actionChange)
        {
            if(actionChange == InputActionChange.ActionStarted)
            {
                _inputDevice = ((InputAction)obj).activeControl.device;

                switch (_inputDevice.device)
                {
                    case Keyboard:
                        _animator.Play("keyboard"); break;
                    case XInputController:
                        _animator.Play("xbox");break;
                }
            }
        }
        private void OnDisable()
        {
            InputSystem.onActionChange -= OnActionChange;
            _playerInput.Gameplay.Interact.started -= OnConfirm;
            HideSign();
        }
        private void Update()
        {
            signSprite.transform.localScale = playerTrans.localScale;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Interactable"))
            {
                DisplaySign(collision);
            }
            else
            {
                HideSign();
            }
        }

        private void DisplaySign(Collider2D collision)
        {
            _canPress = true;
            _targetItem = collision.GetComponent<IInteractable>();
            signSprite.GetComponent<SpriteRenderer>().enabled = true;
            switch (_inputDevice.device)
            {
                case Keyboard:
                    _animator.Play("keyboard"); break;
                case XInputController:
                    _animator.Play("xbox"); break;
            }
        }

        private void HideSign()
        {
            _canPress = false;
            _targetItem = null;
            signSprite.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
