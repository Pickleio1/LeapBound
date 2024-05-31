using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            // If we're in the first scene, restart the game
            GameManager.Instance.RestartGame();
        }
        else
        {
            // Otherwise, load the next scene
            LoadScene(currentSceneIndex + 1);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).buildIndex == 0)
        {
            // If we're loading the first scene, restart the game
            GameManager.Instance.RestartGame();
        }
        else
        {
            // Otherwise, load the requested scene
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}