using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    private Text pointsText;

    private void Start()
    {
        FindAndUpdatePointsText();
        GameManager.Instance.AddPoints(0); // Ensure the GameManager is initialized
    }

    public void EnemyKilled(int points)
    {
        GameManager.Instance.AddPoints(points);
        UpdatePointsText();
        Debug.Log($"Enemy killed, current points: {GameManager.Instance.CurrentPoints}");
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
        FindAndUpdatePointsText();
    }

    private void FindAndUpdatePointsText()
    {
        GameObject pointsDisplayGO = GameObject.FindWithTag("PointsDisplay");
        if (pointsDisplayGO != null)
        {
            pointsText = pointsDisplayGO.GetComponent<Text>();
            UpdatePointsText();
        }
    }

    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = "Points: " + GameManager.Instance.CurrentPoints;
        }
    }
    
}