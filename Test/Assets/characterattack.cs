using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterattack : MonoBehaviour
{
    public int damage = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyhealth enemyhealth = collision.GetComponent<enemyhealth>();
       
        if (enemyhealth != null)
        {
            enemyhealth.TakeDamage(damage);
        }

    }
}
