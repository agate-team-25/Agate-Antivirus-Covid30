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
    [SerializeField] private float sound;
    [SerializeField] private float music;

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
            ChangeSoundVolume(1);
        });

        soundButtonList[1].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 2");
            ChangeSoundVolume(2);
        });

        soundButtonList[2].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 3");
            ChangeSoundVolume(3);
        });

        soundButtonList[3].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 4");
            ChangeSoundVolume(4);
        });

        soundButtonList[4].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 5");
            ChangeSoundVolume(5);
        });

        soundButtonList[5].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 6");
            ChangeSoundVolume(6);
        });

        soundButtonList[6].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 7");
            ChangeSoundVolume(7);
        });

        soundButtonList[7].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 8");
            ChangeSoundVolume(8);
        });

        soundButtonList[8].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 9");
            ChangeSoundVolume(9);
        });

        soundButtonList[9].onClick.AddListener(() =>
        {
            //Debug.Log("Sound volume turned to 10");
            ChangeSoundVolume(10);
        });

        // add listener ke button music volume
        musicButtonList[0].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 1");
            ChangeMusicVolume(1);
        });

        musicButtonList[1].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 2");
            ChangeMusicVolume(2);
        });

        musicButtonList[2].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 3");
            ChangeMusicVolume(3);
        });

        musicButtonList[3].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 4");
            ChangeMusicVolume(4);
        });

        musicButtonList[4].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 5");
            ChangeMusicVolume(5);
        });

        musicButtonList[5].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 6");
            ChangeMusicVolume(6);
        });

        musicButtonList[6].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 7");
            ChangeMusicVolume(7);
        });

        musicButtonList[7].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 8");
            ChangeMusicVolume(8);
        });

        musicButtonList[8].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 9");
            ChangeMusicVolume(9);
        });

        musicButtonList[9].onClick.AddListener(() =>
        {
            //Debug.Log("Music volume turned to 10");
            ChangeMusicVolume(10);
        });

    }

    // Update is called once per frame
    //void Update()
    //{}

    private void OnEnable()
    {
        fullscreenStatus = Screen.fullScreen;
        // Add/call function to get current value of sound and music
        // --GET CURRENT VALUE OF SOUND AND MUSIC--

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
            if (i <= volume)
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
        // --CHANGE VOLUME OF SOUNDFX--
    }

    private void ChangeMusicVolume(float volume)
    {
        //Debug.Log("Music volume turned to " + volume);

        // to show sound button less or equal to volume, and hide the rest
        for (int i = 1; i <= 10; i++)
        {
            if (i <= volume)
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
        // --CHANGE VOLUME OF MUSIC--
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
