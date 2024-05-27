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
    public float fastDropSpeed = 20f;
    public float stunDuration = 2f;
    bool applyFastDrop = false;
    bool isStunned = false;
    float stunTimer = 0;
    public float landingStunDuration = 1f;
    bool wasInAir = false;
    bool quickDropInitiated = false;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
     

    public Rigidbody2D platformRb;
    public Animator animator;

    TouchingDirections touchingDirections;

    public Rigidbody2D rb;

    Vector2 moveInput;
    


    public float CurrentMoveSpeed
    {
        get
        {   if (IsMoving && !touchingDirections.IsOnWall)
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
            animator.SetBool(AnimationStrings.MoveTrigger, value);
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


    private bool _isAttacking = false;
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

    public bool canMove = true;
    public bool CanMove
    {
        get
        {
            return canMove;
        }
        private set
        {
            canMove = value;
        }

    }

    public bool isOnPlatform;
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
        // Quick drop and associated stun can only be initiated if not grounded
        if (Keyboard.current.qKey.wasPressedThisFrame && !isStunned && !touchingDirections.IsGrounded)
        {
            StartAirStun(stunDuration);
        }

        if (isStunned)
        {
            UpdateStun();
        }

        // Check for landing only if quickDropInitiated is true
        if (touchingDirections.IsGrounded && wasInAir && quickDropInitiated)
        {
            StartStun(landingStunDuration); // Apply stun when landing
            quickDropInitiated = false; // Reset the flag after applying stun
        }

        wasInAir = !touchingDirections.IsGrounded; // Update to track if player was in the air last frame
    }

    void FixedUpdate()
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

        if (!isStunned)
        {
            UpdateStun();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Zero out horizontal movement but allow for gravity impact
        }

        animator.SetFloat(AnimationStrings.yvelocity, rb.velocity.y);
   
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
        if (context.started && touchingDirections.IsGrounded && !isStunned)
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

            Debug.Log("attackstarted");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<enemyhealth>().TakeDamage(damage);
                Debug.Log("We hit " + enemy.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return; 

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
      //  if (_isAttacking == true)
     //   {
    //        Debug.Log("IsAttack = true");

    //        if (collision.gameObject.CompareTag("Enemy"))
      //      {
       //         Debug.Log("collision occured");
               // enemyhp.TakeDamage(damage);
    //        }
   //     }
       
    //}



    void StartStun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        rb.velocity = new Vector2(0, rb.velocity.y); // Stop horizontal movement
    }

    void StartAirStun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // Freeze position completely in air
        applyFastDrop = true; // Flag for fast drop after stun
        quickDropInitiated = true; // Indicate that quick drop has been initiated
    }

    void UpdateStun()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            isStunned = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.freezeRotation = true;

            if (applyFastDrop)
            {
                rb.velocity = new Vector2(0, -fastDropSpeed); // Apply fast drop
                applyFastDrop = false;
            }
        }
    }
}