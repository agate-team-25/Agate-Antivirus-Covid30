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

    [Header("Player")]
    public PlayerController Player;

    [Header("Item Spawn")]
    public List<RespawnItem> itemSpawnPoints;

    private bool checkPointTriggered;
    private bool bossKilled;

    // Start is called before the first frame update
    void Start()
    {
        checkPointTriggered = false;
        bossKilled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyBoss == null) 
        {
            bossKilled = true;
        }

        if (Player.isDead)
        {
            ResetSpawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!checkPointTriggered)
        {
            checkPointTriggered = true;
            leftBarrier.SetActive(true);
            rightBarrier.SetActive(true);
            EnemyBoss.SetActive(true);
            Player.GetComponent<PlayerController>().reachCheckPoint = true;
        }
    }

    private void ResetSpawn()
    {
        if (EnemyBoss != null)
        {
            EnemyBoss.GetComponent<EnemyBoss>().ResetEnemyState();
        }

        Player.Resurrect();
        Player.transform.position = transform.position;

        foreach (RespawnItem spawnPoint in itemSpawnPoints)
        {
            spawnPoint.resetTimer();
        }
    }
}
