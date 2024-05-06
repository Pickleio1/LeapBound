using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public float jumpImpulse = 10f;
    public float airWalkSpeed = 3f;
    public float smallJump = 0.7f;

    private bool grounded;

    TouchingDirections touchingDirections;
    public Rigidbody2D rb;

    Vector2 moveInput;
    


    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    return airWalkSpeed;
                }
            }
            else
            {
                return walkSpeed;
            }
        }
    }


    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
        }
    }


    private bool _isRunning = false;
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


    private bool _isAttacking;
    public bool IsAttacking
    {
        get
        {
            return _isAttacking;
        }
        private set
        {
            _isAttacking = value;
        }
    }


    public bool _isFacingRight = false;
    public bool IsFacingRight
    {
        get
        { 
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value) 
            {
                transform.localScale *= new Vector2(-1,1);
            }

            _isFacingRight = value;
        }
    }

    public bool isOnPlatform;
    public Rigidbody2D platformRb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }


    private void FixedUpdate()
    {

        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        if (isOnPlatform)
        {
            rb.velocity = new Vector2((moveInput.x * CurrentMoveSpeed) + platformRb.velocity.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirections(moveInput);
    }


    private void SetFacingDirections(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }


    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }


    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {

            grounded = true;

        }
    }


    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }


    public void OnAttack (InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAttacking = true;
        }
    }
}