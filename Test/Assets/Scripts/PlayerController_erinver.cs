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


    TouchingDirections touchingDirections;

    public Rigidbody2D rb;

    Vector2 moveInput;

    public PowerUpController powerUpController;


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
            animator.SetBool(AnimationStrings.RunTrigger, value);

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

    //public bool CanMove { get
        //{
            //return animator.GetBool(AnimationStrings.canMove);
        //} }

    public bool isOnPlatform;
    public Rigidbody2D platformRb;
    public Animator animator;
    public enemyhealth enemyHealthScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        powerUpController = FindObjectOfType<PowerUpController>();
        enemyHealthScript = FindObjectOfType<enemyhealth>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (isOnPlatform)
        {
        
        rb.freezeRotation = true;
        }

        
        if (Keyboard.current.qKey.wasPressedThisFrame && !isStunned && !touchingDirections.IsGrounded)
        {
            StartAirStun(stunDuration);
        }

        if (isStunned)
        {
            UpdateStun();
        }

        
        if (touchingDirections.IsGrounded && wasInAir && quickDropInitiated)
        {
            StartStun(landingStunDuration); 
            quickDropInitiated = false; 
        }

        wasInAir = !touchingDirections.IsGrounded; 
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
            rb.velocity = new Vector2(0, rb.velocity.y); 
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


    private GameObject projectileGameObject; // Add this line to declare the projectile game object variable

    public bool allowMeleeAttack = true; // Boolean to control melee attack functionality

 public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.AttackTrigger);

            Debug.Log("Attack started");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            if (powerUpController != null)
            {
                if (powerUpController.isProjectilePowerActive && !powerUpController.isProjectilePowerUpgraded)
                {
                    powerUpController.AttemptToShootProjectileBase();
                    // Single projectile shooting when power is active but not upgraded
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        if (enemy.CompareTag("Enemy"))
                        {
                            enemy.GetComponent<enemyhealth>().TakeDamage(damage);
                            Debug.Log("Projectile hit " + enemy.name);
                        }
                    }
                }
                else if (powerUpController.isProjectilePowerActive && powerUpController.isProjectilePowerUpgraded)
                {
                    // Upgraded projectile shooting when power is both active and upgraded
                    powerUpController.AttemptToShootProjectileUpgraded();

                    foreach (Collider2D enemy in hitEnemies)
                    {
                        if (enemy.CompareTag("Enemy"))
                        {
                            enemy.GetComponent<enemyhealth>().TakeDamage(damage);
                            Debug.Log("Projectile hit " + enemy.name);
                        }
                    }
                }
                else
                {
                    // Default melee attack when power is not active
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        if (enemy.CompareTag("Enemy"))
                        {
                            enemy.GetComponent<enemyhealth>().TakeDamage(damage);
                            Debug.Log("Melee hit " + enemy.name);
                        }
                    }
                }
            }
        }
    }

    public void OnTeleport(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        if (powerUpController != null && powerUpController.isTeleportActive)
        {
            powerUpController.AttemptToTeleport();
            Debug.Log("Player teleported.");
        }
        else
        {
            Debug.Log("Teleport power not active.");
        }
    }

    private void DestroyProjectile()
    {
        // Add logic to destroy the projectileGameObject
        if (projectileGameObject != null)
        {
            Destroy(projectileGameObject); // Destroy the projectile game object
        }
    }

    private void OnDrawGizmos()
    {
        

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



    void StartStun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        rb.velocity = new Vector2(0, rb.velocity.y); 
    }

    void StartAirStun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        applyFastDrop = true; 
        quickDropInitiated = true; 
    }

    public float GetVerticalInput()
    {
        return moveInput.y;
    }

    void UpdateStun()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            isStunned = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (applyFastDrop)
            {
                rb.velocity = new Vector2(0, -fastDropSpeed); 
                applyFastDrop = false;
            }
        }
    }

    void ApplyQuickDrop()
    {
        if (quickDropInitiated)
        {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Re-confirm rotation lock
        rb.velocity = new Vector2(0, -fastDropSpeed); // Quick drop only affects vertical speed
        quickDropInitiated = false;  // Reset quick drop state
        }
    }

    public Vector2 GetFacingDirection()
    {
        if (IsFacingLeft)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }
}