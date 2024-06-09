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


    public void Awake()
    {

    }


    public void Setup()
    {
        Debug.Log("Setup called");
        gameOverScreen.SetActive(true);
    }

    public void TryAgain()
    {
        levelLoader.LoadLevel(levelIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu(int levelIndex)
    {
        levelLoader.LoadLevel(levelIndex);
    }
}
