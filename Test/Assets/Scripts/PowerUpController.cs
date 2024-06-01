using UnityEngine;
using System.Collections;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null); // Detach from parent to become root-level
            DontDestroyOnLoad(gameObject); // Apply DontDestroyOnLoad to the root-level object
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        // Find Player and ShootingPoint by tag
        if (playerController == null)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            if (playerObjects.Length > 0)
            {
                playerController = playerObjects[0].GetComponent<PlayerController>();
            }

            if (playerController == null)
            {
                Debug.LogError("PlayerController not found. Ensure it is present in the scene with the 'Player' tag.");
            }
        }

        if (shootingPoint == null && playerController != null)
        {
            shootingPoint = playerController.transform.Find("ShootingPoint");
            if (shootingPoint == null)
            {
                Debug.LogError("ShootingPoint not found under Player. Check the hierarchy.");
            }
        }
    }

    private IEnumerator FindPlayerAndShootingPointWithDelay()
    {
        yield return new WaitForSeconds(1f); // Wait for a short delay, adjust as needed

        // Assign the playerController reference dynamically
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found. Ensure it is present in the scene.");
        }
        
        // Find the shooting point recursively in the player hierarchy
        if (playerController != null)
        {
            Transform shootingPointTransform = SearchForShootingPoint(playerController.transform);
            if (shootingPointTransform != null)
            {
                shootingPoint = shootingPointTransform;
            }
            else
            {
                Debug.LogError("ShootingPoint not found in the player hierarchy.");
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
    }

    public void ToggleProjectilePower()
    {
        isProjectilePowerActive = !isProjectilePowerActive;
    }

    public void UpgradeProjectilePower()
    {
        if (isProjectilePowerActive)
        {
            isProjectilePowerUpgraded = true;
        }
    }

    public void AttemptToShootProjectile()
    {
        if (projectilePrefab == null || shootingPoint == null)
        {
            return;
        }

        if (Time.time - lastProjectileTime >= projectileCooldown)
        {
            ShootProjectile();
            lastProjectileTime = Time.time;
        }
    }

    private void ShootProjectile()
    {
        Debug.Log("Shooting Projectile");

        if (projectilePrefab == null || shootingPoint == null)
        {
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);

        // Set the projectile's movement
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        if (projectileRigidbody != null)
        {
            Vector2 projectileDirection = playerController.GetFacingDirection();
            projectileRigidbody.velocity = projectileDirection * 10f;
            Debug.Log("Projectile direction: " + projectileDirection);
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

                    // Destroy the projectile on collision
                    Destroy(projectile);

                    yield break; // Exit the coroutine after handling the collision
                }
            }

            yield return null; // Wait for the next frame to check for collisions
        }

        // Add a return statement here to handle the case where the while loop ends
        yield return null;
    }
}