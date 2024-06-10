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
    Animator animator;
    public float invincibilityDuration = 2f;
    private bool isInvincible = false;
    public Animator Death;

    public bool isDead = false;
    public AudioManager audioManager;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
        private set
        {
            isDead = value;
            animator.SetBool(AnimationStrings.IsDead, value);
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        currentLife = 3; // Set the starting current life
        maxLives = 5; // Set the maximum lives threshold
        animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    void Update()
    {
        if (currentLife <= 0 && !IsDead)
        {
            IsDead = true;
            audioManager.PlaySFX(audioManager.die);
            SceneManager.LoadScene("Game Over");
            Debug.Log("Player is dead. Triggering GameOverScreen.");
        }
        
        Debug.Log("Current Lives: " + currentLife);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentLife -= damage;
            animator.SetTrigger(AnimationStrings.HitTrigger);
            audioManager.PlaySFX(audioManager.takedamage);
            UpdateHealthUI();
            StartCoroutine(InvincibilityCooldown());
        }
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
        if (collision.gameObject.CompareTag("Player") && !isInvincible)
        {
            TakeDamage(1);
        }

        if (collision.gameObject.CompareTag("Spike") && !isInvincible)
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

    private IEnumerator InvincibilityCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public void Invincibility()
    {
        StartCoroutine(InvincibilityCooldown());
    }
}