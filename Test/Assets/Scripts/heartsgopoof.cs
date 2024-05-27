using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class heartsgopoof : MonoBehaviour
{
    
    public int maxlives;
    private int currentlife;
    public GameObject bullet;
    public GameObject[] heart;
    public timerscript timer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentlife = PlayerPrefs.GetInt("life");

        currentlife = 3;
        
       
    }

    
    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("life", currentlife);
        
        if (currentlife < 1)   //destroy hearts 
        {
            isDead();     //stop timer when dead
        }
        else if (currentlife <2)
        {
            Destroy(heart[1].gameObject);
        }
        else if (currentlife < 3)
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

    public void isDead()
    {
        Destroy(heart[0].gameObject) ;  
        Destroy(gameObject);
        timerscript.Destroy(timer);


    }

    public void NextScene()   //to change between scenes
    {
        SceneManager.LoadScene(1);
    }

    public void PreviousScene()
    {
        SceneManager.LoadScene(0);
    }

    
}
