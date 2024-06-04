using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{    
    public enum InteractionType { NONE, PickUp, Examine, GrabDrop, Consume }
    public enum ItemType { Static, Consumables }
    
    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;
    
    [Header("Examine")]
    public string descriptionText;
    
    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private InteractionSystem interactionSystem; // Reference to InteractionSystem script

    private void Start()
    {
        interactionSystem = FindObjectOfType<InteractionSystem>(); // Find and store the InteractionSystem script reference
    }

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractionType.PickUp:
                // Add the object to the PickedUpItems list
                // Disable
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                interactionSystem.ExamineItem(this);
                break;
            case InteractionType.GrabDrop:
                interactionSystem.GrabDrop();
                break;
            case InteractionType.Consume:
                consumeEvent.Invoke(); // Trigger the consume event
                if (type == ItemType.Consumables)
                {
                    if (interactionSystem != null)
                    {
                        interactionSystem.AddLives(1); // Add 1 life when the consumable is consumed
                    }
                }
                gameObject.SetActive(false); // Disable the item after consumption
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        customEvent.Invoke(); // Invoke the custom event(s)
    }
}