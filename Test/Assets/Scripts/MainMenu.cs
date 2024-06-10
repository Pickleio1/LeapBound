using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject Options;
    public Animator animator;

    private bool _mainMenu;

    public bool mainMenu {  
        get { return _mainMenu; } private set {  _mainMenu = value; animator.SetBool(AnimationStrings.MainMenu, value);}
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
        mainMenu = true;
        gameManager = FindObjectOfType<GameManager>();
    }

    public void PlayGame()
    {
        mainMenu = false;
        SceneManager.LoadScene("Level 1");
        gameManager.ResetPoints();
        
    }

    public void QuitGame()
    {
        mainMenu = false;

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