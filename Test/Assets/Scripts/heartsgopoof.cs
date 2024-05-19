using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heartsgopoof : MonoBehaviour
{
    
    public int maxlives;
    private int currentlife;
    public GameObject bullet;
    public GameObject[] heart;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentlife = maxlives;
        
    }

    
    // Update is called once per frame
    void Update()
    {
        if (currentlife == 0)   //destroy hearts 
        {
            Destroy(heart[0].gameObject);
            Destroy(gameObject);   //destroy player
        }
        else if (currentlife == 1)
        {
            Destroy(heart[1].gameObject);
        }
        else if (currentlife == 2)
        {
            Destroy(heart[2].gameObject);
        }
    }

    public void TakeDamage(int damage) //take dmg
    {
        currentlife -= damage;
        
    }

    private void OnTriggerEnter2D(Collider2D collision) //decrease life
    {
        if (bullet.gameObject.CompareTag("Player"))
        {
            currentlife -= 1;
        }

       
    }


}
