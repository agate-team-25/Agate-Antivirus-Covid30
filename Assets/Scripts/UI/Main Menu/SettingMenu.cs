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
    [SerializeField] bool fullscreenStatus;
    [SerializeField] float sound;
    [SerializeField] float music;

    // Color untuk menghilangkan/memunculkan button
    private Color32 activeColor = new Color32(255, 255, 225, 225);
    private Color32 inactiveColor = new Color32(255, 255, 225, 0);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ChangeFullScreen(fullscreenStatus);

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

    private void ChangeSoundVolume(int volume)
    {

    }

    private void ChangeMusicVolume(int volume)
    {

    }
}
