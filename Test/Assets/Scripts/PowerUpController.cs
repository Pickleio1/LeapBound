using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PowerUpController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject forceFieldPrefab;
    private GameObject forcefieldGameObject;
    public Transform shootingPoint;
    public int projectileDamage = 1; // Damage value of the projectile

    [SerializeField] public bool isProjectilePowerActive = false;
    [SerializeField] public bool isProjectilePowerUpgraded = false;

    public bool isTeleportActive = false;
    public bool isTeleportUpgraded = false;
    public bool isForcefieldPowerActive = false;
    public bool isForcefieldPowerUpgraded = false;
    public float forcefieldCooldownBase = 5f;
    public float forcefieldCooldownUpgraded = 3f;
    private float lastForcefieldActivationTime = -5f;
    public float forcefieldDuration = 3f;
    public float teleportCooldownBase = 5f;
    public float teleportCooldownUpgraded = 4f;
    public float teleportDistanceBase = 4f;
    public float teleportDistanceUpgraded = 5f;

    public bool isInvulnerable = false;

    public float projectileCooldown = 5f;
    public float lastProjectileTime = -5f;
    public PlayerController playerController;

    // Singleton instance
    private static PowerUpController instance;
    public static PowerUpController Instance { get { return instance; } }

    public float projectileAngle = 45f; // Angle of the additional projectiles

    // Teleport cooldown tracking
    private float lastTeleportTime = -5f;

    // Teleport direction options
    private Vector3 teleportDirectionBase = new Vector3(4f, 4f, 4f);
    private Vector3 teleportDirectionUpgraded = new Vector3(5f, 5f, 5f);

    private Tilemap tilemap;
    public heartsgopoof Heartsgopoof;
    public TouchingDirections touchingDirections;
    private Transform playerTransform;
    public Rigidbody2D rb;


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

        Heartsgopoof = FindObjectOfType<heartsgopoof>();
        touchingDirections = FindObjectOfType<TouchingDirections>();
        rb = FindObjectOfType<Rigidbody2D>();
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
        FindTilemap();
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
            playerTransform = playerController.transform;
            shootingPoint = playerTransform.Find("ShootingPoint");
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
        Debug.Log("Teleport Power Active: " + isTeleportActive );
        Debug.Log("Teleport Power Upgrade Active: " + isTeleportUpgraded);
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

    public void UpgradeTeleport()
    {
        if (isTeleportActive)
        {
            isTeleportUpgraded = true;
            Debug.Log("Teleport power successfully upgraded");
        }
        else
        {
            Debug.Log("Activate base teleport power before upgrading.");
        }
    }

    // Toggle Teleport power
    public void ToggleTeleportPower()
    {
        isTeleportActive = !isTeleportActive;
    }

    public void ToggleForceFieldPower()
    {
        isForcefieldPowerActive = !isForcefieldPowerActive;
    }

    public void UpgradeForceFieldPower()
    {
        if (isForcefieldPowerActive == true)
        {
            isForcefieldPowerUpgraded =!isForcefieldPowerUpgraded;
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

    private void FindTilemap()
    {
        // Find the Tilemap GameObject in the scene
        GameObject tilemapObject = GameObject.FindWithTag("Ground");
        if (tilemapObject != null)
        {
            // Get the Tilemap component from the GameObject
            tilemap = tilemapObject.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Tilemap GameObject not found in the scene. Make sure there is a GameObject with the 'Tilemap' tag.");
        }
    }

    // Teleport the player with the specified distance and direction
private void PerformTeleport(Transform playerTransform, Vector2 facingDirection, float verticalInput)
{
    float teleportDistance = isTeleportUpgraded ? teleportDistanceUpgraded : teleportDistanceBase;
    Vector3 teleportVector = GetTeleportDirectionVector(facingDirection, verticalInput, teleportDistance);

    // Check if the target position is inside the Tilemap
    Vector3 targetPosition = playerTransform.position + teleportVector;
    if (IsPositionInTilemap(targetPosition))
    {
        // Adjust the teleport distance to be just before the Tilemap
        float distanceToTilemap = GetDistanceToTilemap(playerTransform.position, teleportVector.normalized);
        if (distanceToTilemap < teleportDistance)
        {
            teleportDistance = distanceToTilemap - 0.1f; // Add a small offset to ensure the player doesn't get stuck in the Tilemap
            targetPosition = playerTransform.position + (teleportVector.normalized * teleportDistance);

            // Check if the adjusted target position is still inside the Tilemap
            if (IsPositionInTilemap(targetPosition))
            {
                // The teleport is still blocked by the Tilemap, so teleport the player to the closest position in front of the Tilemap
                targetPosition = playerTransform.position + (teleportVector.normalized * (distanceToTilemap - 0.1f));
            }
        }
        else
        {
            // The Tilemap is too far away, so don't perform the teleport
            return;
        }
    }

    // Check if there are any obstacles between the player's current position and the target position
    RaycastHit2D[] hits = Physics2D.LinecastAll(playerTransform.position, targetPosition);
    bool obstacleFound = false;
    float closestObstacleDistance = float.MaxValue;
    Vector3 closestObstaclePosition = Vector3.zero;

    foreach (RaycastHit2D hit in hits)
    {
        if (hit.collider != null && hit.collider.gameObject != playerTransform.gameObject)
        {
            obstacleFound = true;
            float distanceToObstacle = Vector3.Distance(playerTransform.position, hit.point);
            if (distanceToObstacle < closestObstacleDistance)
            {
                closestObstacleDistance = distanceToObstacle;
                closestObstaclePosition = Vector3.Lerp(hit.point, targetPosition, 0.1f);
            }
        }
    }

    if (obstacleFound)
    {
        // Teleport the player to the closest position in front of the obstacle
        playerTransform.position = closestObstaclePosition;
        Debug.Log("Player teleported in front of obstacle.");
    }
    else
    {
        // Teleport the player to the target position
        playerTransform.position = targetPosition;
        Debug.Log("Player teleported.");
    }
}
    private float GetDistanceToTilemap(Vector3 origin, Vector3 direction)
    {
        // Raycast from the player's position in the direction of the teleport
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, ~0);

        // If the ray hits something, return the distance to the hit point
        if (hit.collider != null)
        {
            return hit.distance;
        }
        else
        {
            // If the ray doesn't hit anything, return a large distance (e.g., 100 units)
            return 100f;
        }
    }

    private bool IsPositionInTilemap(Vector3 position)
    {
        // Check if the Tilemap has been found
        if (tilemap != null)
        {
            // Get the tile at the target position
            Vector3Int tilePosition = tilemap.WorldToCell(position);
            TileBase tile = tilemap.GetTile(tilePosition);

            // Check if the tile is not null (i.e., there is a tile at the target position)
            return tile != null;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetTeleportDirectionVector(Vector2 facingDirection, float verticalInput, float teleportDistance)
    {
        Vector3 teleportVector = Vector3.zero;

        if (isTeleportUpgraded)
        {
            // 8-directional teleportation
            if (facingDirection.x < 0 && verticalInput > 0)
            {
                teleportVector = new Vector3(-1, 1, 0) * teleportDistance; // North-West
            }
            else if (facingDirection.x < 0 && verticalInput < 0)
            {
                teleportVector = new Vector3(-1, -1, 0) * teleportDistance; // South-West
            }
            else if (facingDirection.x > 0 && verticalInput > 0)
            {
                teleportVector = new Vector3(1, 1, 0) * teleportDistance; // North-East
            }
            else if (facingDirection.x > 0 && verticalInput < 0)
            {
                teleportVector = new Vector3(1, -1, 0) * teleportDistance; // South-East
            }
            else if (facingDirection.x < 0)
            {
                teleportVector = Vector3.left * teleportDistance; // West
            }
            else if (facingDirection.x > 0)
            {
                teleportVector = Vector3.right * teleportDistance; // East
            }
            else if (verticalInput > 0)
            {
                teleportVector = Vector3.up * teleportDistance; // North
            }
            else if (verticalInput < 0)
            {
                teleportVector = Vector3.down * teleportDistance; // South
            }
        }
        else
        {
            // 4-directional teleportation
            if (Mathf.Abs(facingDirection.x) > Mathf.Abs(verticalInput))
            {
                // Prioritize horizontal input
                if (facingDirection.x < 0)
                {
                    teleportVector = Vector3.left * teleportDistance;
                }
                else
                {
                    teleportVector = Vector3.right * teleportDistance;
                }
            }
            else
            {
                // Prioritize vertical input
                if (verticalInput > 0)
                {
                    teleportVector = Vector3.up * teleportDistance;
                }
                else if (verticalInput < 0)
                {
                    teleportVector = Vector3.down * teleportDistance;
                }
            }
        }

        return teleportVector;
    }

    public void AttemptToTeleport()
    {
        if (Time.time - lastTeleportTime >= (isTeleportUpgraded ? teleportCooldownUpgraded : teleportCooldownBase))
        {
            Vector2 facingDirection = playerController.GetFacingDirection();
            float verticalInput = playerController.GetVerticalInput();
            PerformTeleport(playerController.transform, facingDirection, verticalInput);
            lastTeleportTime = Time.time;
        }
        else
        {
            Debug.Log("Teleport cooldown not ready.");
        }
    }
    public void ActivateForcefield()
    {
        if (isForcefieldPowerActive && Time.time - lastForcefieldActivationTime >= (isForcefieldPowerUpgraded ? forcefieldCooldownUpgraded : forcefieldCooldownBase))
        {
            if (isPlayerGrounded() || isForcefieldPowerUpgraded)
            {


                StartCoroutine(TriggerForcefield());
                lastForcefieldActivationTime = Time.time;
            }
            else
            {
                Debug.Log("Player must be grounded to activate the upgraded forcefield mid-air.");
            }
        }
        else
        {
            Debug.Log("Forcefield cooldown not ready.");
        }
    }
    private IEnumerator TriggerForcefield()
    {
        // Display circle forcefield around the player
        ShowForcefield();

        // Store the player's current velocity to restore later
        Vector2 originalVelocity = playerController.rb.velocity;

        // Set the player's velocity to a reduced speed
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        // Make player invincible
        SetInvulnerable(true);

        yield return new WaitForSeconds(forcefieldDuration);

        // Reset invincibility status after forcefield ends
        SetInvulnerable(false);

        // Restore the player's original velocity
        playerController.rb.velocity = originalVelocity;

        // Hide the forcefield
        HideForcefield();
    }
    private bool isPlayerGrounded()
    {
        if (touchingDirections.IsGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowForcefield()
    {
        // Instantiate the forcefield prefab and attach it to the player
        forcefieldGameObject = Instantiate(forceFieldPrefab, playerTransform.position, Quaternion.identity);
        forcefieldGameObject.transform.SetParent(playerTransform);
    }
    private void HideForcefield()
    {
        // Destroy the forcefield GameObject
        Destroy(forcefieldGameObject);
    }

    private void SetInvulnerable(bool invulnerable)
    {
        Heartsgopoof.Invincibility();
        isInvulnerable = invulnerable;
    }
}