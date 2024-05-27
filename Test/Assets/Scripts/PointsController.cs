using UnityEngine;
using UnityEngine.UI;
public class PointsController : MonoBehaviour
{
    public int currentPoints = 0;
    public Text pointsText;

    private void Start()
    {
        UpdatePointsText();
    }

    public void EnemyKilled()
    {
        currentPoints += 10;
        UpdatePointsText();
    }

    private void UpdatePointsText()
    {
        pointsText.text = "Points: " + currentPoints;
    }
}