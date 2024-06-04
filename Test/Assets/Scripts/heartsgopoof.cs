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

    private static bool IsSet;
    
    
    // Start is called before the first frame update
    void Start()
    {

        if (IsSet = false)
        {
            currentlife = PlayerPrefs.GetInt("lifeu", maxlives);
            IsSet = true;
            DontDestroyOnLoad(gameObject);  // Prevent this object from being destroyed when loading a new scene
        }
        else
        {
            currentlife = PlayerPrefs.GetInt("lifeu", currentlife);
        }

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
            isDead();     


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


        PlayerPrefs.SetInt("lifeu", currentlife);
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
        heart[0].gameObject.SetActive(false) ;  //destroy last heart

        Destroy(gameObject);    //destroy player

        timerscript.Destroy(timer);    //stops timer when dead
    }

    void OnApplicationQuit()  //reset life
    {
        PlayerPrefs.DeleteKey("lifeu");

    }

    

    
}
