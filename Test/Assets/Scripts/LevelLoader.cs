using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public int loadLevelIndex = 1; // Default value is 1
    public bool requireKillAllEnemies = false; // New variable to toggle the feature
    public bool requireKeyItemInteraction = false; // New variable to toggle the key item interaction feature
    public int requiredPointsIncrease = 10; // New variable to set the required points increase
    public GameObject[] requiredKeyItems; // New variable to assign the required key items in the Unity editor

    public int previousPointsCount = 0; // Track the points count when the level was last loaded
    public bool hasInteractedWithAllKeyItems = false; // Track if the player has interacted with all the required key items
    private GameManager gameManager;
    public List<GameObject> interactedKeyItems = new List<GameObject>(); // List to store interacted key items
    public GameObject killEnemiesCanvas;
    public GameObject findKeyItemsCanvas;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
            
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (loadLevelIndex < 10)
            {
                if (requireKillAllEnemies)
                {
                    // Check if the player's points have increased by the required amount
                    if (gameManager.CurrentPoints >= previousPointsCount + requiredPointsIncrease)
                    {
                        LoadLevel(loadLevelIndex + 1);
                        previousPointsCount = gameManager.CurrentPoints;
                        killEnemiesCanvas.SetActive(false); // Hide the "kill enemies" canvas
                        findKeyItemsCanvas.SetActive(false); // Hide the "find key items" canvas
                    }
                    else
                    {
                        // Display a message to the player, or do something else
                        Debug.Log($"You must increase your points by at least {requiredPointsIncrease} to load the next level.");
                        killEnemiesCanvas.SetActive(true); // Show the "kill enemies" canvas
                        findKeyItemsCanvas.SetActive(false); // Hide the "find key items" canvas
                    }
                }
                else if (requireKeyItemInteraction)
                {
                    // Check if the player has interacted with all the required key items
                    if (hasInteractedWithAllKeyItems)
                    {
                        LoadLevel(loadLevelIndex + 1);
                        killEnemiesCanvas.SetActive(false); // Hide the "kill enemies" canvas
                        findKeyItemsCanvas.SetActive(false); // Hide the "find key items" canvas
                    }
                    else
                    {
                        // Display a message to the player, or do something else
                        Debug.Log($"You must interact with all the required key items to load the next level.");
                        killEnemiesCanvas.SetActive(false); // Hide the "kill enemies" canvas
                        findKeyItemsCanvas.SetActive(true); // Show the "find key items" canvas
                    }
                }
                else
                {
                    // Load the next level without any requirements
                    LoadLevel(loadLevelIndex + 1);
                    killEnemiesCanvas.SetActive(false); // Hide the "kill enemies" canvas
                    findKeyItemsCanvas.SetActive(false); // Hide the "find key items" canvas
                }
            }
        }
    }

    public void KeyItemInteracted(GameObject keyItem)
    {
        // Check if the key item is in the list of required key items
        for (int i = 0; i < requiredKeyItems.Length; i++)
        {
            if (requiredKeyItems[i] == keyItem)
            {
                interactedKeyItems.Add(keyItem);
                if (interactedKeyItems.Count == requiredKeyItems.Length)
                {
                    hasInteractedWithAllKeyItems = true;
                }
                break;
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelCoroutine(levelIndex));
    }

    IEnumerator LoadLevelCoroutine(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void HideKillEnemiesCanvas()
    {
        killEnemiesCanvas.SetActive(false);
    }

    public void HideFindKeyItemsCanvas()
    {
        findKeyItemsCanvas.SetActive(false);
    }
}