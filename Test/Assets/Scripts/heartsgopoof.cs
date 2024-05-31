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
    Animator animator;
    public GameOverScreen gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        currentlife = maxlives;
        animator = GetComponent<Animator>();    
    }


    // Update is called once per frame
    void FixedUpdate()
    {



    }

    public void TakeDamage(int damage) //dmg and destroy player
    {
        currentlife -= damage;
        animator.SetTrigger(AnimationStrings.HitTrigger);
        
        if (currentlife < 0)
        {
            gameManager.gameOver
            Destroy(gameObject);
        }
            
    }

 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);  // Existing bullet damage logic
        }

        if (collision.gameObject.CompareTag("Spike"))  // Assuming spikes are tagged as "Spike"
        {
            TakeDamage(1);  // Call TakeDamage with the damage value that spikes should inflict
        }
        UpdateHealthUI();  // Call this method to update health display if you have one
    }

    // Assumes you have a method to update the health UI
    private void UpdateHealthUI()
    {
        for (int i = 0; i < heart.Length; i++)
        {
            if (i < currentlife)
                heart[i].SetActive(true);  // Show heart if health is above index
            else
                heart[i].SetActive(false);  // Hide heart if health is below or equal to index
        }
    }
}
