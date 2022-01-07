using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region singleton
    public static LevelManager _instance = null;
    public static LevelManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }

            return _instance;
        }
    }
    #endregion

    [Header("Enemies")]
    //public Transform[] enemiesPoint;
    public List<Enemy> enemies = new List<Enemy>();
    public int targetKilled = 10;

    private int allEnemies;
    private int enemyKilled;

    [Header("Text")]
    public Text timerText;
    public Text enemyKilledText;
    public Text powerUpLevel;
    public Text statusText;

    [Header("Win Text")]
    public Text timerTextWin;
    public Text enemyKilledTextWin;

    [Header("Lose Text")]
    public Text timerTextLose;
    public Text enemyKilledTextLose;

    [Header("UI Object")]
    public GameObject winUI;
    public GameObject loseUI;

    [Header("Status Bar")]
    public GameObject APDBar;
    public GameObject bleedBar;
    public GameObject feverBar;
    public GameObject GunCd;
    public GameObject DesinfektanCd;

    public float timeRemaining;
    public bool timerIsRunning = false;
    public bool enableInput;

    // Start is called before the first frame update
    void Start()
    {
        // Memastikan data telah ke load di start menu, jika tidak maka load ulang
        if (UserDataManager.Progress == null)
        {
            UserDataManager.Load();
            //Debug.Log("player telah memiliki progress sampai level :" + UserDataManager.Progress.levelProgress);
        }

        timerIsRunning = true;
        enableInput = true;
        allEnemies = GetEnemyCount();
        powerUpLevel.text = "Level 0";
        statusText.text = "Healthy";
        statusText.color = new Color(0f, 202f / 255f, 11f / 255f);
    }

    // Update is called once per frame
    void Update()
    {
        #region TImer
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                LoseCondition();
            }
        }

        DisplayTime(timeRemaining);
        #endregion

        enemyKilled = allEnemies - GetEnemyCount();
        enemyKilledText.text = "" + enemyKilled + "/" + targetKilled;
        //Debug.Log("remaining enemies :" + allEnemies);
        if (PlayerController.instance != null)
        {
            int powerLevel = PlayerController.instance.powerUpLevel;
            powerUpLevel.text = "Level " + powerLevel;

            if(powerLevel == 1)
            {                
                GunCd.SetActive(true);
                DesinfektanCd.SetActive(false);
            }

            else if(powerLevel == 2)
            {                
                DesinfektanCd.SetActive(true);
                GunCd.SetActive(false);
            }

            else
            {
                GunCd.SetActive(false);
                DesinfektanCd.SetActive(false);
            }

            if (PlayerController.instance.status == "Fever")
            {
                statusText.text = "Fever";
                statusText.color = new Color(48f / 255f, 48f / 255f, 111f / 255f);
                feverBar.SetActive(true);
            }
            else if (PlayerController.instance.status == "Bleed")
            {
                statusText.text = "Bleed";
                statusText.color = new Color(1f, 0f, 0f);
                bleedBar.SetActive(true);
            }

            else
            {
                statusText.text = "Healthy";
                statusText.color = new Color(0f, 202f / 255f, 11f / 255f);
                feverBar.SetActive(false);
                bleedBar.SetActive(false);
            }

            if (PlayerController.instance.GetAPDStatus())
            {
                APDBar.SetActive(true);
                feverBar.SetActive(false);
                bleedBar.SetActive(false);
            }

            else
            {
                APDBar.SetActive(false);
            }
        }
        
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public int GetEnemyCount()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }

        return enemies.Count;
    }

    public void OnWin()
    {
        //Debug.Log("You win");
        timerTextWin.text = timerText.text;
        enemyKilledTextWin.text = enemyKilledText.text;
        enableInput = false;
        Pause.instance.Paused(winUI);

        if (UserDataManager.Progress.levelProgress < 1)
        {
            UserDataManager.Progress.levelProgress = 1;
            UserDataManager.Save();
        }
    }

    public void OnLose()
    {
        timerTextLose.text = timerText.text;
        enemyKilledTextLose.text = enemyKilledText.text;
        enableInput = false;
        Pause.instance.Paused(loseUI);
    }

    private void LoseCondition()
    {
        if(timeRemaining <= 0 && enemyKilled <= allEnemies)
        {
            OnLose();
        }
    }

    public int GetEnemyOffset()
    {
        return allEnemies - targetKilled;
    }
}
