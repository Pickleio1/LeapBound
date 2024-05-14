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
    public int damage = 1;
    public enemyhealth enemyHealth;
    


    TouchingDirections touchingDirections;
    public Rigidbody2D rb;

    Vector2 moveInput;
    


    public float CurrentMoveSpeed
    {
        get
        {   if (CanMove)
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
                {   //Speed into Wall = 0
                    return 0;
                }
            } else
            {   //Locked Movement, Can Move is False, Animation Bool Behaviour
                return 0;
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


    public bool _isFacingLeft = false;
    public bool IsFacingLeft
    {
        get
        { 
            return _isFacingLeft;
        }
        private set
        {
            if (_isFacingLeft != value) 
            {
                transform.localScale *= new Vector2(-1,1);
            }

            _isFacingLeft = value;
        }
    }

    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } }

    public bool isOnPlatform;
    public Rigidbody2D platformRb;
    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
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
        if (FindObjectOfType<InteractionSystem>()!= null && FindObjectOfType<InteractionSystem>().isExamining)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        if (isOnPlatform)
        {
            rb.velocity = new Vector2((moveInput.x * CurrentMoveSpeed) + platformRb.velocity.x, rb.velocity.y);
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
        if (moveInput.x < 0 && !IsFacingLeft)
        {
            IsFacingLeft = true;
        }
        else if (moveInput.x > 0 && IsFacingLeft)
        {
            IsFacingLeft = false;
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
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


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.AttackTrigger);
            IsAttacking = true;

        }
    }


    private void OnCollisionEnter2D (Collision2D enemyCol)
    {
        if (enemyCol.gameObject.tag == "Enemy")
        {
           enemyHealth.TakeDamage(damage);
       }
   }
}