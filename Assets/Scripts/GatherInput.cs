using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{

    public Controls myControls;

    //X axis input direction
    public float valueX;

    //Identify Jump Input
    public bool jumpInput;

    private void Awake()
    {
        myControls = new Controls();
    }

    private void OnEnable()
    {
        myControls.Player.Move.performed += StartMove;
        myControls.Player.Move.canceled += StopMove;

        myControls.Player.Jump.performed += JumpStart;
        myControls.Player.Jump.canceled += JumpStop;

        myControls.Player.Enable();
    }

    private void OnDisable()
    {
        myControls.Player.Move.performed -= StartMove;
        myControls.Player.Move.canceled -= StopMove;

        myControls.Player.Jump.performed -= JumpStart;
        myControls.Player.Jump.canceled -= JumpStop;

        myControls.Player.Disable();
    }

    private void StartMove(InputAction.CallbackContext ctx)
    {
        valueX = ctx.ReadValue<float>();
    }
    private void StopMove(InputAction.CallbackContext ctx)
    {
        valueX = 0;
    }

    private void JumpStart(InputAction.CallbackContext ctx)
    {
        jumpInput = true;
    }
    private void JumpStop(InputAction.CallbackContext ctx)
    {
        jumpInput = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
        }
    }
}
