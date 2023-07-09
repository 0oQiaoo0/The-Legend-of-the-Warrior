using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    private PlayerInoutControl playerInput;
    private Animator animator;
    public Transform playerTrans;
    public GameObject signSprite;

    private IInteractable targetItem;
    private bool canPress;

    private InputDevice inputDevice;

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        animator = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInoutControl();
        playerInput.Enable();

        inputDevice = new InputDevice();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Interact.started += OnConfirm;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
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
            inputDevice = ((InputAction)obj).activeControl.device;

            switch (inputDevice.device)
            {
                case Keyboard:
                    animator.Play("keyboard"); break;
                case XInputController:
                    animator.Play("xbox");break;
            }
        }
    }
    private void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
        playerInput.Gameplay.Interact.started -= OnConfirm;
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
        canPress = true;
        targetItem = collision.GetComponent<IInteractable>();
        signSprite.GetComponent<SpriteRenderer>().enabled = true;
        switch (inputDevice.device)
        {
            case Keyboard:
                animator.Play("keyboard"); break;
            case XInputController:
                animator.Play("xbox"); break;
            default:break;
        }
    }

    private void HideSign()
    {
        canPress = false;
        targetItem = null;
        signSprite.GetComponent<SpriteRenderer>().enabled = false;
    }
}
