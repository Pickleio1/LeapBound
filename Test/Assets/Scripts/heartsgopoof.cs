using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class heartsgopoof : MonoBehaviour
{
    public int maxLives;
    private int currentLife;
    public GameObject bullet;
    public GameObject[] heart;
    Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentLife = maxLives;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentLife <= 0 && !isDead)
        {
            isDead = true;
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
        currentLife += livesToAdd;
        if (currentLife > maxLives)
        {
            currentLife = maxLives;
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
            }
            else
            {
                heart[i].SetActive(false);
            }
        }
    }
}