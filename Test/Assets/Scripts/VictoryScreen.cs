using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class VictoryScreen : MonoBehaviour
{
    GameManager gameManager;
    public GameObject VictoryScreenUI;

    public void Awake()
    {
        VictoryScreenUI.SetActive(true);
    }

    public void OnPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.RestartGame();
        }
    }
}
