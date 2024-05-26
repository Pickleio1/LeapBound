using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyhealth : MonoBehaviour
{
    public int maxhealth = 3;
    private int currenthealth;
    public int damage = 1;
    //Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxhealth;
        // col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    

    public void TakeDamage(int damage) //dmg and destroy enemy
    {
        currenthealth -= damage;
        if (currenthealth <= 0)
        {
            Destroy(gameObject);
        }
       
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
            Debug.Log("Enemy collided with another enemy and took damage");
        }
    }
}
