using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class heartsgopoof : MonoBehaviour
{
    public int maxLives;
    public int currentLife;
    public GameObject bullet;
    public GameObject[] heart;
    public timerscript timer;
    Animator animator;
    private bool isDead = false;

    private static bool Initialized;


    void Start()
    {
        currentLife = 3; // Set the starting current life
        maxLives = 5; // Set the maximum lives threshold
        animator = GetComponent<Animator>();
        UpdateHealthUI();


        if (Initialized == false)
        {
            currentLife = PlayerPrefs.GetInt("lifeu", maxLives);
            Initialized = true;
            DontDestroyOnLoad(gameObject);  // Prevent player from being destroyed when loading a new scene
        }
        else
        {
            currentLife = PlayerPrefs.GetInt("lifeu", currentLife);
        }

        UpdateHealthUI();
    }

    void Update()
    {
        
        if (currentLife <= 0 && !isDead)
        {
            isDead = true;
            PlayerPrefs.DeleteKey("lifeu");
            Initialized = false;

            Destroy(gameObject);    //destroy player

            timerscript.Destroy(timer);    //stops timer when dead

            SceneManager.LoadScene("Game Over");
            Debug.Log("Player is dead. Triggering GameOverScreen.");
        }
        
        Debug.Log("Current Lives: " + currentLife);
    }

    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        animator.SetTrigger(AnimationStrings.HitTrigger);
        UpdateHealthUI();
    }

    public void AddLives(int livesToAdd)
    {
        if (currentLife + livesToAdd > maxLives)
        {
            currentLife = maxLives;
        }
        else
        {
            currentLife += livesToAdd;
        }
        
        UpdateHealthUI();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < heart.Length; i++)
        {
            if (i < currentLife)
            {
                heart[i].SetActive(true);
                PlayerPrefs.SetInt("lifeu", currentLife);
            }
            else
            {
                heart[i].SetActive(false);
                PlayerPrefs.SetInt("lifeu", currentLife);
            }
        }
    }

    void OnApplicationQuit()  //reset life
    {
        PlayerPrefs.DeleteKey("lifeu");

    }

}