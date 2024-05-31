using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public GameOverScreen gameOverScreen;

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
