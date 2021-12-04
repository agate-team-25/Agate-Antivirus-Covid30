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
    public Transform[] enemiesPoint;
    public Enemy[] enemies;

    [Header("Items")]
    public Transform[] itemsPoint;
    public Items[] items;

    private int allEnemies;
    private int enemyKilled;

    [Header("Text")]
    public Text timerText;
    public Text allEnemiesText;
    public Text enemyKilledText;
    public Text powerUpLevel;

    public float timeRemaining = 120;
    public bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
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
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }

        DisplayTime(timeRemaining);
        #endregion
    }

    void InstantiateAllEnemies()
    {
        
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
