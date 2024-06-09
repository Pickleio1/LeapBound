using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyhealth : MonoBehaviour
{
    public int maxhealth = 3;
    private int currenthealth;
    public int damage = 1;
    public int pointsForKill = 10;
    public Animator animator;
    private bool _enemyIsDead = false;
    private Rigidbody rb;

    public PointsController pointsController ;

    public void Awake()
    {
        pointsController = FindObjectOfType<PointsController>();
    }

    public bool IsDead
    {
        get { return _enemyIsDead; } private set
        {
            _enemyIsDead = value;
            animator.SetBool(AnimationStrings.enemyIsDead, value);

        }

    }

    void StartCoroutine()
    {
        StartCoroutine(Die(3));
    }

    IEnumerator Die(float duration)
    {
        IsDead = true;

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);

        pointsController.EnemyKilled(pointsForKill); 

    }


    private void Start()
    {
        currenthealth = maxhealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }

    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        animator.SetTrigger(AnimationStrings.enemyHit);

        if (currenthealth <= 0)
        {
            StartCoroutine();
            rb.AddForce(Vector3.down * 10f);
            Debug.Log("Enemy died.");
            FindObjectOfType<PointsController>().EnemyKilled(pointsForKill); // Call the EnemyKilled() function in the PointsController script
            Debug.Log($"Calling EnemyKilled() with {pointsForKill} points");
        }
    }
}