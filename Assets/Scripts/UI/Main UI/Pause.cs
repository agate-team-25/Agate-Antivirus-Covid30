using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pause : MonoBehaviour
{
    #region singleton
    public static Pause _instance = null;
    public static Pause instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Pause>();
            }

            return _instance;
        }
    }
    #endregion

    public static bool GamePaused = false;
    public GameObject PauseMenuUI;
    private string sceneName;

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = true;
        GamePaused = false;
    }

    public void Paused(GameObject ui)
    {
        ui.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = false;
        GamePaused = true;
    }

    public void Paused()
    {
        Paused(PauseMenuUI);
    }

    public void OnBack()
    {

    }

    public void OnRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }

    public void OnPlay()
    {

    }

    public void OnNext()
    {

    }
}
