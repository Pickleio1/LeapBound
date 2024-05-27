using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D collision)       //literally just giving 1 damage to player
    {
        if (collision.gameObject.tag == "Player")
        {
            playerhealth.TakeDamage(damage);

        }
    }
}
