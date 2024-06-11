using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class timerscript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timertext;
    public float elapsedtime;
    public bool counting;



    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("time"))
        {
            elapsedtime = PlayerPrefs.GetFloat("time");
        }


    }

    // Update is called once per frame
    void Update()
    {
        elapsedtime += Time.deltaTime;

        PlayerPrefs.SetFloat("time", elapsedtime);

        float minutes = Mathf.FloorToInt(elapsedtime / 60);
        float seconds = Mathf.FloorToInt(elapsedtime % 60);
        timertext.text = string.Format("{0:00}:{1:00}", minutes, seconds);   // make timer look like 0:00
    }

    public void StopTimer()
    {
        counting = false;
    }

    private void OnApplicationQuit()  //reset timer
    {
        PlayerPrefs.DeleteKey("time");
    }


}
