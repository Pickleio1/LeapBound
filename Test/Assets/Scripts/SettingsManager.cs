using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject SettingsMenuUI;
    private void Awake()
    {
        SettingsMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused) 
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        
    }

    public void Resume()
    {

        SettingsMenuUI.SetActive(false);
        Time.timeScale = 0f;
        GamePaused = false;
    }

    public void Pause()
    {
        SettingsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }
}

