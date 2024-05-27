using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyhealth : MonoBehaviour
{
    public int maxhealth = 3;
    private int currenthealth;
    public int damage = 1;

    private void Start()
    {
        currenthealth = maxhealth;
    }

    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        if (currenthealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy died.");
            FindObjectOfType<PointsController>().EnemyKilled(); // Call the EnemyKilled() function in the PointsController script
        }
    }
}