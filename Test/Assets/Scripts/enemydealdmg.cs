using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydealdmg : MonoBehaviour
{
    public heartsgopoof playerhealth;
    public int damage = 1;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerhealth.TakeDamage(damage);

        }
    }
}
