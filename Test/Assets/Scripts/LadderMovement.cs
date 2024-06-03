using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    public float ClimbingSpeed = 8f;
    public float JumpForce = 10f; // Jump force value
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
    private float originalGravityScale;  // Store the original gravity

    void Start() 
    {
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
        else if (!isLadder) // Ensure to stop climbing when not on the ladder
        {
            isClimbing = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isClimbing)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, vertical * ClimbingSpeed);
        }
        else
        {
            rb.gravityScale = originalGravityScale; // Reset the gravity scale
        }
    }

    private void Jump()
    {
        isClimbing = false; // Disable climbing when jumping
        rb.velocity = new Vector2(rb.velocity.x, JumpForce); // Apply jump force
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false; // Stop climbing when exiting the ladder
        }
    }
}