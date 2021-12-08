using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    #region variables
    [Header("FullScreen Settings")]
    [SerializeField] private Button switchOn;
    [SerializeField] private Text onText;
    [SerializeField] private Button switchOff;
    [SerializeField] private Text offText;

    [Header("Sound FX Settings")]
    [SerializeField] private Button[] soundButtonList = new Button[10];

    [Header("Music Settings")]
    [SerializeField] private Button[] musicButtonList = new Button[10];

    [Header("Settings Default Value")]
    [SerializeField] private bool fullscreenStatus;
    [Range(0f, 1f)]
    [SerializeField] private float sound;
    [Range(0f, 1f)]
    [SerializeField] private float music;

    // to save audio manager
    private AudioManager audioManager;

    // Color untuk menghilangkan/memunculkan button
    private Color32 activeColor = new Color32(255, 255, 225, 225);
    private Color32 inactiveColor = new Color32(255, 255, 225, 0);
    #endregion

    #region singleton
    // Untuk menjadikan object singleton
    private static SettingMenu _instance = null;
    public static SettingMenu Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SettingMenu>();
            }

            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        // Get audio manager
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // add listener ke button switch on/off fullscreen
        switchOn.onClick.AddListener(() =>
        {
            //Debug.Log("Full Screen On");
            ChangeFullScreen(true);
        });
        switchOff.onClick.AddListener(() =>
        {
            //Debug.Log("Full Screen Off");
            ChangeFullScreen(false);
        });

        // add listener ke button sound volume
        soundButtonList[0].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 1");
            ChangeSoundVolume(0.1f);
        });

        soundButtonList[1].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 2");
            ChangeSoundVolume(0.2f);
        });

        soundButtonList[2].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 3");
            ChangeSoundVolume(0.3f);
        });

        soundButtonList[3].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 4");
            ChangeSoundVolume(0.4f);
        });

        soundButtonList[4].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 5");
            ChangeSoundVolume(0.5f);
        });

        soundButtonList[5].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 6");
            ChangeSoundVolume(0.6f);
        });

        soundButtonList[6].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 7");
            ChangeSoundVolume(0.7f);
        });

        soundButtonList[7].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 8");
            ChangeSoundVolume(0.8f);
        });

        soundButtonList[8].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 9");
            ChangeSoundVolume(0.9f);
        });

        soundButtonList[9].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 10");
            ChangeSoundVolume(1f);
        });

        // add listener ke button music volume
        musicButtonList[0].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 1");
            ChangeMusicVolume(0.1f);
        });

        musicButtonList[1].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 2");
            ChangeMusicVolume(0.2f);
        });

        musicButtonList[2].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 3");
            ChangeMusicVolume(0.3f);
        });

        musicButtonList[3].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 4");
            ChangeMusicVolume(0.4f);
        });

        musicButtonList[4].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 5");
            ChangeMusicVolume(0.5f);
        });

        musicButtonList[5].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 6");
            ChangeMusicVolume(0.6f);
        });

        musicButtonList[6].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 7");
            ChangeMusicVolume(0.7f);
        });

        musicButtonList[7].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 8");
            ChangeMusicVolume(0.8f);
        });

        musicButtonList[8].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 9");
            ChangeMusicVolume(0.9f);
        });

        musicButtonList[9].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 10");
            ChangeMusicVolume(1f);
        });

    }

    // Update is called once per frame
    //void Update()
    //{}

    private void OnEnable()
    {
        // To get current status of fullscreen
        fullscreenStatus = Screen.fullScreen;
        
        // to make sure the audio manager is stil referenced
        if (audioManager == null)
        {
            //Debug.Log("Audio Manager not found");
            audioManager = FindObjectOfType<AudioManager>();
        }

        // Add/call function to get current value of sound and music
        music = audioManager.musicVol;
        sound = audioManager.soundVol;

        ChangeFullScreen(fullscreenStatus);
        ChangeSoundVolume(sound);
        ChangeMusicVolume(music);
    }

    private void ChangeFullScreen(bool status)
    {
        // method untuk mengganti game ke fullscreen atau tidak
        Screen.fullScreen = status;

        // mengubah tampilan switch berdasarkan status fullscreen
        if (status)
        {
            switchOn.image.color = activeColor;
            onText.color = activeColor;
            switchOff.image.color = inactiveColor;
            offText.color = inactiveColor;
        }

        else
        {
            switchOn.image.color = inactiveColor;
            onText.color = inactiveColor;
            switchOff.image.color = activeColor;
            offText.color = activeColor;
        }

        fullscreenStatus = status;
    }

    private void ChangeSoundVolume(float volume)
    {
        //Debug.Log("Sound volume turned to " + volume);

        // to show sound button less or equal to volume, and hide the rest
        for (int i=1; i <= 10; i++)
        {
            // multiply vol by 10 to similarize the scale
            float vol10 = volume * 10;

            if (i <= vol10)
            {
                soundButtonList[i - 1].image.color = activeColor;
            }
            else
            {
                soundButtonList[i - 1].image.color = inactiveColor;
            }
        }

        // change sound value
        sound = volume;

        // Add/call function to change volume of soundfx here
        audioManager.SetSoundVol(volume);
    }

    private void ChangeMusicVolume(float volume)
    {
        //Debug.Log("Music volume turned to " + volume);

        // to show sound button less or equal to volume, and hide the rest
        for (int i = 1; i <= 10; i++)
        {
            // multiply vol by 10 to similarize the scale
            float vol10 = volume * 10;

            if (i <= vol10)
            {
                musicButtonList[i - 1].image.color = activeColor;
            }
            else
            {
                musicButtonList[i - 1].image.color = inactiveColor;
            }
        }

        // change music value
        music = volume;

        // Add/call function to change volume of music here
        audioManager.SetMusicVol(volume);
    }

    public float CurrentSoundValue()
    {
        return sound;
    }

    public float CurrentMusicValue()
    {
        return music;
    }
}
