using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class heartsgopoof : MonoBehaviour
{
    
    public int maxlives;
    private int currentlife = 3;
    public GameObject bullet;
    public GameObject[] heart;
    public timerscript timer;
    public GameObject add;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        DecreaseLife();
    }

    
    // Update is called once per frame
    void Update()
    {
        
        DecreaseLife();
    }

    public void DecreaseLife()
    {
        if (currentlife < 1)   //destroy hearts 
        {
            isDead();     //stop timer when dead
        }
        else if (currentlife < 2)
        {
            heart[1].gameObject.SetActive(false);

        }
        else if (currentlife < 3)
        {
            heart[2].gameObject.SetActive(false);

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

    public void isDead() //die
    {
        heart[0].gameObject.SetActive(false) ;  
        Destroy(gameObject);
        timerscript.Destroy(timer);
    }

    void OnApplicationQuit()  //reset life
    {
        PlayerPrefs.DeleteKey("lifeu");
    }

    public void AddLife() //heal
    {
        if (currentlife < maxlives)
        {
            currentlife += 1;
            
            if (currentlife == 2)
            {
                heart[1].gameObject.SetActive(true);
            }
            else if (currentlife == 3)
            {
                heart[2].gameObject.SetActive(true) ;
            }
        }
        
    }

    
}
