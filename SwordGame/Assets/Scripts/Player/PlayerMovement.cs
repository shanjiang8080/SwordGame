using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jumpButtonDown = false;
    bool jumpJustPressed = false;

    public PivotMovement pivotMovement;

    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }
    // Update is called once per frame
    void Update()
    {
        horizontalMove = moveAction.ReadValue<Vector2>().x * runSpeed;
        if (jumpAction.IsPressed())
        {
            jumpButtonDown = true;
        }
        if (jumpAction.WasPressedThisFrame())
        {
            jumpJustPressed = true;
        } 
    }
    void FixedUpdate()
    {
        // move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jumpButtonDown, jumpJustPressed, !pivotMovement.isThrown);
        jumpButtonDown = false;
        jumpJustPressed = false;

    }
}
