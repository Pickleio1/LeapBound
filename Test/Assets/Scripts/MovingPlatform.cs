using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    Vector3 targetPos;

    PlayerController movementController;
    Rigidbody2D rb;
    Vector3 moveDirection;

    private void Awake()
    {
        movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    } 

    // Start is called before the first frame update
    void Start()
    {
        targetPos = posB.position;
        DirectionCalculate();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            targetPos = posB.position;
            DirectionCalculate();
        }

        if (Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            targetPos = posA.position;
            DirectionCalculate();
        }

    }

    private void FixedUpdate()
    {
        if (!movementController.IsAttacking &&!Mathf.Approximately(movementController.rb.velocity.y, 0f))
        {
            rb.velocity = moveDirection * speed;
        }
    }

    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            movementController.isOnPlatform = true;
            movementController.platformRb = rb;
        }
    }

        private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            movementController.isOnPlatform = false;
        }
    }
}
