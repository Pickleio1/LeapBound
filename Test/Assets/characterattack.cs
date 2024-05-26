using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterattack : MonoBehaviour
{
    public int damage = 1;
    public enemyhealth enemyhp;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyhp.TakeDamage(damage); 
        //enemyhealth enemyhealth = collision.GetComponent<enemyhealth>();

        //if (enemyhp != null)
        //{
           // Debug.Log("Enemy health component found, applying damage");
           // enemyhealth.TakeDamage(damage);
       // }
       // else
       // {
           // Debug.Log("Enemy health component not found");
       // }



    }
}
