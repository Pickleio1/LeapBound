using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip backgroundAlive;
    public AudioClip backgroundDead;
    public AudioClip backgroundMenu;
    public AudioClip buysuccess;
    public AudioClip buyfail;
    public AudioClip takedamage;
    public AudioClip die;
    public AudioClip teleport;
    public AudioClip Melee;
    public AudioClip Quickdrop;
    public AudioClip projectileshoot;
    public AudioClip forcefield;
    public AudioClip jump;

    private static AudioManager instance;
    private void Awake()
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



    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    private void Start()
    {
        UpdateBackgroundMusic();
    }

    private void Update()
    {
        UpdateBackgroundMusic();
    }

    private void UpdateBackgroundMusic()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Game Over")
        {
            if (musicSource.clip != backgroundDead)
            {
                musicSource.clip = backgroundDead;
                musicSource.Play();
            }
        }
        if (currentScene == "Main Menu")
        {
            if (musicSource.clip != backgroundMenu)
            {
                musicSource.clip = backgroundMenu;
                musicSource.Play();
            }
        }
        else
        {
            if (musicSource.clip != backgroundAlive)
            {
                musicSource.clip = backgroundAlive;
                musicSource.Play();
            }
        }
    }
}