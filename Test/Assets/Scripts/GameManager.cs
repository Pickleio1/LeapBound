using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int CurrentPoints { get; private set; }

    private void Awake()
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
    }

    public void AddPoints(int points)
    {
        CurrentPoints += points;
        SavePoints();
    }

    private void SavePoints()
    {
        // Save the points to PlayerPrefs or a persistent data storage system
        PlayerPrefs.SetInt("PointsKey", CurrentPoints);
    }

    private void LoadPoints()
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
        // Reset any necessary game state
        CurrentPoints = 0;
        SavePoints();

        // Load the first scene
        SceneController.instance.LoadScene(0);
    }
}