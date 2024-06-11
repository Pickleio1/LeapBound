using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.TimeZoneInfo;
using static UnityEngine.UI.Selectable;


public class SettingsManager : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject SettingsMenuUI;
    public LevelLoader levelLoader;



    private void Awake()
    {
        GamePaused = false;
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
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause()
    {
        SettingsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }


    public void SaveChanges()
    {

    }

    public void MainMenu()
    {
        SettingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        SceneManager.LoadScene("Main Menu");
    }



}
