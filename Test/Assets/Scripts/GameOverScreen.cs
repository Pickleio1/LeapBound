using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameManager gameManager;
    public LevelLoader levelLoader;
    private int levelIndex = 1;
    public timerscript timerScript;


    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();  
        timerScript = FindObjectOfType<timerscript>();
    }


    public void Setup()
    {
        Debug.Log("Setup called");
        gameOverScreen.SetActive(true);
    }

    public void TryAgain()
    {
        gameManager.RestartGame();
        timerScript.ResetTimer();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu(int levelIndex)
    {
        gameManager.ResetPoints();
        SceneManager.LoadScene("Main Menu");
    }
}
