using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject Options;

   
    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
        gameManager.ResetPoints();
        
    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public void OnButtonPress()
    {

        Options.SetActive(true);
    }

    public void OnButtonExit()
    {
        Options.SetActive(false);
    }
}