using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public int projectileDamage = 1; // Damage value of the projectile

    [SerializeField] public bool isProjectilePowerActive = false;
    [SerializeField] public bool isProjectilePowerUpgraded = false;
    public float projectileCooldown = 5f;
    public float lastProjectileTime = -5f;
    public PlayerController playerController;

    // Singleton instance
    private static PowerUpController instance;
    public static PowerUpController Instance { get { return instance; } }

    public float projectileAngle = 45f; // Angle of the additional projectiles

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerAndShootingPoint();
    }

    private void FindPlayerAndShootingPoint()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found. Ensure it is present in the scene.");
        }

        if (playerController != null)
        {
            shootingPoint = playerController.transform.Find("ShootingPoint");
            if (shootingPoint == null)
            {
                Debug.LogError("ShootingPoint not found under Player. Check the hierarchy.");
            }
        }
    }

    private Transform SearchForShootingPoint(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == "ShootingPoint")
            {
                return child;
            }

            Transform result = SearchForShootingPoint(child);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    void Update()
    {
        Debug.Log("Power-Up Active: " + isProjectilePowerActive);
        Debug.Log("Power-Up Upgrade Active: " + isProjectilePowerUpgraded);
    }

    public void ToggleProjectilePower()
    {
        isProjectilePowerActive = !isProjectilePowerActive;
    }

    public void UpgradeProjectilePower()
    {
        if (isProjectilePowerActive == true)
        {
            isProjectilePowerUpgraded = !isProjectilePowerUpgraded;
            Debug.Log("Power-Upgrade successfully upgraded");
        }
        else
        {
            Debug.Log("Power-Upgrade failed. Base power must be active to upgrade.");
        }
    }

    private void ShootProjectileBase()
    {
        if (projectilePrefab == null || shootingPoint == null)
        {
            return;
        }

        if (isProjectilePowerActive == true)
        {
            // Original single projectile firing behavior
            FireProjectile(0f);
        }
    }

    private void ShootProjectileUpgraded()
    {
            if (isProjectilePowerUpgraded == true && isProjectilePowerActive == true)
        {
            // Fire three projectiles (0 degrees, -45 degrees, 45 degrees)
            FireProjectile(0f);
            FireProjectile(-projectileAngle);
            FireProjectile(projectileAngle);
        } 
        else if (isProjectilePowerUpgraded == true && isProjectilePowerActive == false)
        {
            Debug.Log("Cannot use upgraded power without activating base power.");
        }
    }

    public void AttemptToShootProjectileBase()
    {
        if (projectilePrefab == null || shootingPoint == null)
        {
            return;
        }

        if (Time.time - lastProjectileTime >= projectileCooldown)
        {
            ShootProjectileBase();
            lastProjectileTime = Time.time;
        }
    }
    public void AttemptToShootProjectileUpgraded()
    {
        if (projectilePrefab == null || shootingPoint == null)
        {
            return;
        }

        if (Time.time - lastProjectileTime >= projectileCooldown)
        {
            ShootProjectileUpgraded();
            lastProjectileTime = Time.time;
        }
    }

    private void FireProjectile(float angleOffset)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        
        // Rotate the projectile based on the angle offset
        projectile.transform.Rotate(0f, 0f, angleOffset);

        // Set the projectile's movement
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        if (projectileRigidbody != null)
        {
            Vector2 projectileDirection = playerController.GetFacingDirection();
            Vector2 direction = Quaternion.Euler(0, 0, angleOffset) * projectileDirection;
            projectileRigidbody.velocity = direction * 10f;
            Debug.Log("Projectile direction: " + direction);
        }

        // Check for collision with any objects
        StartCoroutine(ProjectileCollisionDetection(projectile));
    }

    private IEnumerator ProjectileCollisionDetection(GameObject projectile)
    {
        // Get the projectile's collider to exclude it from collision checks
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        Collider2D playerCollider = playerController.GetComponent<Collider2D>(); // Get player's collider

        while (projectile != null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(projectile.transform.position, 0.5f);

            foreach (Collider2D hitCollider in hitColliders)
            {
                // Check if the hit collider is not the projectile or player collider
                if (hitCollider != projectileCollider && hitCollider != playerCollider)
                {
                    // Handle collision with any object other than the projectile and player
                    Debug.Log("Projectile collided with: " + hitCollider.name);

                    if (hitCollider.CompareTag("Enemy"))
                    {
                        // Deal damage to the enemy
                        enemyhealth enemy = hitCollider.GetComponent<enemyhealth>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(projectileDamage);
                        }
                    }
                    else if (hitCollider.CompareTag("Ground"))
                    {
                        // Destroy the projectile only if it hits Ground or Enemy
                        Destroy(projectile);
                    }

                    yield break; // Exit the coroutine after handling the collision
                }
            }

            yield return null; // Wait for the next frame to check for collisions
        }

        // Add a return statement here to handle the case where the while loop ends
        yield return null;
    }
}