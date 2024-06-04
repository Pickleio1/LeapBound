using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameManager gameManager;

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
        GameManager.Instance.RestartGame();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
