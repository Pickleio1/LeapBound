using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostArea : MonoBehaviour
{
    public float speedMultiplier = 2.0f; // Factor to multiply the speed
    public float durationAfterExit = 2.0f; // Duration to keep the speed after exiting the area

    private bool isPlayerInside = false; // Flag to check if the player is inside the area
    private bool boostApplied = false; // Flag to prevent reapplication of the boost
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

        if (!isPlayerInside) // Check if the player is no longer inside the area when delay ends
        {
            boostApplied = false; // Allow speed boost to be applied again upon re-entry
            // Reset player's speeds
            player.walkSpeed /= speedMultiplier;
            player.runSpeed /= speedMultiplier;
            player.airWalkSpeed /= speedMultiplier;
        }
    }
}