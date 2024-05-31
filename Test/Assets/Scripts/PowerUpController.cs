using System.Collections;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform shootingPoint; 

    [SerializeField]
    public bool isProjectilePowerActive = false; 
    [SerializeField]
    public bool isProjectilePowerUpgraded = false; 
    [SerializeField]
    private float projectileCooldown = 5f; 
    private float lastProjectileTime = -5f; 

    void Update() {
        //Debug.Log("Power-Up Active: " + isProjectilePowerActive);
    }


    
    public void ToggleProjectilePower()
    {
        isProjectilePowerActive = !isProjectilePowerActive;
        if (!isProjectilePowerActive)
        {
            
            isProjectilePowerUpgraded = false;
        }
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
        if (projectilePrefab == null) {
            Debug.LogError("Projectile prefab not assigned.");
            return;
        }
        if (shootingPoint == null) {
            Debug.LogError("Shooting point not assigned.");
            return;
        }

        if (Time.time - lastProjectileTime >= projectileCooldown )
        {
            Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
            lastProjectileTime = Time.time;
        }
    }
    private void ShootProjectile()
    {
        Debug.Log("Shooting Projectile");
        var projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        Debug.Log("Projectile instantiated: " + projectile);
        if (projectilePrefab == null) Debug.Log("Projectile Prefab is null");
        if (shootingPoint == null) Debug.Log("Shooting point is null");
        if (projectilePrefab && shootingPoint)
        {
            Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
            // If power-up is upgraded, perhaps shoot multiple projectiles
            if (isProjectilePowerUpgraded)
            {
                
            }
        }
    }
}