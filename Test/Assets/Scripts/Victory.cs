using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Victory : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public LevelLoader levelLoader;
    private int levelIndex = 0;

    public void Awake()
    {
        videoPlayer = FindObjectOfType<VideoPlayer>();
        videoPlayer.loopPointReached += CheckOver;
    }

   
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("video over");
        MainMenu(levelIndex);

    }

    void MainMenu(int levelIndex)
    {
        levelLoader.LoadLevel(levelIndex);

    }
}


