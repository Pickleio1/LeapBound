using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostArea : MonoBehaviour
{
    public float speedMultiplier = 2.0f; 
    public float durationAfterExit = 2.0f; 

    private bool isPlayerInside = false; 
    private bool boostApplied = false; 
    private Coroutine resetSpeedCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && !boostApplied)
        {
            isPlayerInside = true;
            boostApplied = true;
            // Apply speed boost
            player.walkSpeed *= speedMultiplier;
            player.runSpeed *= speedMultiplier;
            player.airWalkSpeed *= speedMultiplier;
        }
        else if (resetSpeedCoroutine != null)
        {
            StopCoroutine(resetSpeedCoroutine);
            resetSpeedCoroutine = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            isPlayerInside = false;
            resetSpeedCoroutine = StartCoroutine(ResetSpeed(player, durationAfterExit));
        }
    }

    private IEnumerator ResetSpeed(PlayerController player, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isPlayerInside) 
        {
            boostApplied = false; 
            
            player.walkSpeed /= speedMultiplier;
            player.runSpeed /= speedMultiplier;
            player.airWalkSpeed /= speedMultiplier;
        }
    }
}