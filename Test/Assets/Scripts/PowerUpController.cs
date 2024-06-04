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
        if (isProjectilePowerUpgraded && isProjectilePowerActive)
        {
            // Fire the upgraded projectiles
            FireProjectile(0f);
            FireProjectile(-projectileAngle);
            FireProjectile(projectileAngle);
        } 
        else if (isProjectilePowerUpgraded && !isProjectilePowerActive)
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

    private IEnumerator ProjectileCollisionDetection(GameObject projectile)
    {
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        Collider2D playerCollider = playerController.GetComponent<Collider2D>();

        // Ignore collisions between the projectile's collider and the player's collider
        Physics2D.IgnoreCollision(projectileCollider, playerCollider, true);

        while (projectile != null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(projectile.transform.position, 0.5f);

            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider != projectileCollider && hitCollider != playerCollider)
                {
                    Debug.Log("Projectile collided with: " + hitCollider.name);

                    if (hitCollider.CompareTag("Enemy"))
                    {
                        enemyhealth enemy = hitCollider.GetComponent<enemyhealth>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(projectileDamage);
                        }
                    }

                    if (hitCollider.CompareTag("Ground") || hitCollider.CompareTag("Enemy"))
                    {
                        Destroy(projectile);
                        break;
                    }
                }
            }

            yield return null;
        }

        // Re-enable collisions between the projectile's collider and the player's collider
        Physics2D.IgnoreCollision(projectileCollider, playerCollider, false);
    }
    private void FireProjectile(float angleOffset)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);

        // Rotate the projectile sprite based on the player's facing direction with horizontal inversion
        Vector2 playerDirection = playerController.GetFacingDirection();
        float angle = Mathf.Atan2(-playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        if (projectileRigidbody != null)
        {
            Vector2 direction = Quaternion.Euler(0, 0, angleOffset) * playerDirection;
            projectileRigidbody.velocity = direction * 10f;
            Debug.Log("Projectile direction: " + direction);
        }

        StartCoroutine(ProjectileCollisionDetection(projectile));
    }
}