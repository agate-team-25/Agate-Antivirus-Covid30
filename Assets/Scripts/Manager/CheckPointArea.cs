using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointArea : MonoBehaviour
{
    [Header("Barrier")]
    public GameObject leftBarrier;
    public GameObject rightBarrier;

    [Header("Boss")]
    public GameObject EnemyBoss;
    public GameObject bossHealthBar;

    [Header("Player")]
    public PlayerController Player;

    [Header("Item Spawn")]
    public List<RespawnItem> itemSpawnPoints;

    [Header("Notification")]
    public GameObject notificationUI;
    public float UIUptime = 5;

    private bool checkPointTriggered;
    private bool bossKilled;
    private bool killReqReached;
    private float UICounter;

    // Start is called before the first frame update
    void Start()
    {
        checkPointTriggered = false;
        bossKilled = false;
        killReqReached = false;
        UICounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyBoss == null)
        {
            bossKilled = true;
            StartCoroutine(CallOnWin());
        }

        if (Player.isDead && checkPointTriggered && Player != null)
        {
            ResetSpawn();
        }

        if (!killReqReached)
        {
            CheckKillRequirement();
        }

        if (UICounter > 0)
        {
            UICounter -= Time.deltaTime;
            if (UICounter <= 0)
            {
                notificationUI.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!checkPointTriggered && !bossKilled)
        {
            FindObjectOfType<AudioManager>().StopMusic("Level1_Music");
            FindObjectOfType<AudioManager>().PlayMusic("Boss_Music");
            checkPointTriggered = true;
            leftBarrier.SetActive(true);
            leftBarrier.GetComponent<BossAreaBarrier>().checkpointActive = true;
            rightBarrier.SetActive(true);
            EnemyBoss.SetActive(true);
            bossHealthBar.SetActive(true);
            Player.GetComponent<PlayerController>().reachCheckPoint = true;
            notificationUI.SetActive(true);
            UICounter = UIUptime;
        }
    }

    private void ResetSpawn()
    {
        if (!bossKilled || EnemyBoss != null)
        {
            EnemyBoss.GetComponent<EnemyBoss>().ResetEnemyState();
        }

        Player.Resurrect();
        Player.transform.position = transform.position;

        foreach (RespawnItem spawnPoint in itemSpawnPoints)
        {
            if (!spawnPoint.CheckItemAvaibility()) 
            {
                spawnPoint.resetTimer();
            }
        }
    }

    private void CheckKillRequirement()
    {
        int enemyCount = LevelManager.instance.GetEnemyCount();
        int offSet = LevelManager.instance.GetEnemyOffset();

        if (enemyCount <= offSet)
        {
            leftBarrier.SetActive(false);
            killReqReached = true;
        }
    }

    IEnumerator CallOnWin()
    {
        yield return new WaitForSeconds(1.5f);
        LevelManager.instance.OnWin();
    }
}
