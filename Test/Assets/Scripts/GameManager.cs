using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public int CurrentPoints { get; set; }

    public PowerUpController powerUpController;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Load the points from PlayerPrefs or a persistent data storage system
        LoadPoints();
        powerUpController = FindObjectOfType<PowerUpController>();
    }

    public void AddPoints(int points)
    {
        CurrentPoints += points;
        SavePoints();
    }

    public void SavePoints()
    {
        // Save the points to PlayerPrefs or a persistent data storage system
        PlayerPrefs.SetInt("PointsKey", CurrentPoints);
    }

    public void LoadPoints()
    {
        // Load the points from PlayerPrefs or a persistent data storage system
        CurrentPoints = PlayerPrefs.GetInt("PointsKey", 0);
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
        Debug.Log($"Loaded scene: {scene.name} | Current points: {CurrentPoints}");
    }

    public void ResetPoints()
    {
        CurrentPoints = 0;
        SavePoints();
    }

   public void RestartGame()
    {
        Debug.Log("Restarting");
        
        // Reset any necessary game state
        ResetPoints(); // Reset the current points to 0
        
        // Load the first scene
        SceneManager.LoadScene(1);
    }
}