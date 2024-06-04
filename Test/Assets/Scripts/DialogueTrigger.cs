using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueScript;
    public Transform teleportPoint; // Variable to store the teleport point
    private bool playerDetected;

    // Detect trigger with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we triggered the player, enable playerDetected and show indicator
        if (collision.tag == "Player")
        {
            playerDetected = true;
            dialogueScript.ToggleIndicator(playerDetected);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we lost trigger with the player, disable playerDetected, hide indicator, and end dialogue
        if (collision.tag == "Player")
        {
            playerDetected = false;
            dialogueScript.ToggleIndicator(playerDetected);
            dialogueScript.EndDialogue();

            // Teleport player if teleportPoint is assigned
            if (teleportPoint != null)
            {
                TeleportPlayer();
            }
        }
    }

    // While detected, if we interact, start the dialogue
    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            dialogueScript.StartDialogue();
        }
    }

    private void TeleportPlayer()
    {
        // Teleport the player to the assigned teleportPoint
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = teleportPoint.position;
        }
    }
}