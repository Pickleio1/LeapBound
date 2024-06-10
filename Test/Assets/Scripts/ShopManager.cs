using System.Collections;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private PowerUpController powerUpController;
    private bool isInitialized = false;

    private void Start()
    {
        StartCoroutine(InitializePowerUpController());
    }

    private IEnumerator InitializePowerUpController()
    {
        while (powerUpController == null)
        {
            GameObject powerUpObject = GameObject.FindGameObjectWithTag("PowerUp");
            if (powerUpObject != null)
            {
                powerUpController = powerUpObject.GetComponent<PowerUpController>();
                isInitialized = true;
            }
            else
            {
                yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before trying again
            }
        }
    }

    public void BuyProjectilePowerBase()
    {
        if (isInitialized && powerUpController != null)
        {
            powerUpController.buyProjectilepowerBase();
        }
    }
    public void BuyProjectilePowerUpgraded()
    {
        if (powerUpController != null)
        {
            powerUpController.buyProjectilePowerUpgraded();
        }
    }

    public void BuyForcefieldPowerBase()
    {
        if (powerUpController != null)
        {
            powerUpController.buyForcefieldPowerBase();
        }
    }

    public void BuyForcefieldPowerUpgraded()
    {
        if (powerUpController != null)
        {
            powerUpController.buyForcefieldPowerUpgraded();
        }
    }

    public void BuyTeleportPowerBase()
    {
        if (powerUpController != null)
        {
            powerUpController.buyTeleportPowerBase();
        }
    }

    public void BuyTeleportPowerUpgraded()
    {
        if (powerUpController != null)
        {
            powerUpController.buyTeleportPowerUpgraded();
        }
    }
}