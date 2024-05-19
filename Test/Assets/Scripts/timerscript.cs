using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class timerscript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timertext;
    public float elapsedtime;
    public bool counting;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedtime += Time.deltaTime;
        float minutes = elapsedtime / 60;
        float seconds = elapsedtime % 60;
        timertext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        
        counting = false;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(1);
    }

    public void PreviousScene()
    {
        SceneManager.LoadScene(0);
    }
}
