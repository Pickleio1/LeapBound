using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody2D), typeof(TouchingDirections))]

public class playercontroller : MonoBehaviour
{
    public float walkSpeed = 5F;
    public float runSpeed = 8F;
    public float jumpPower = 4F;

    TouchingDirections touchingDirections;
    Rigidbody2D rb;

    Vector2 moveInput;

    private bool _isMoving = false;
    private bool _isRunning = false;

    public float CurrentMoveSpeed {  get
        {
            if (IsMoving)
            {
                if (IsRunning)
                {
                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            } else
                {
                    return 0;
                }
            }
        }

    
    public bool IsMoving { get
        {
            return _isMoving;
        } private set
        {
            _isMoving = value;
        }
    }

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed , rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;

        } else if (context.canceled)  
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //make sure to check if alive
        if (context.started)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

    }

   
}
