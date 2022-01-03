using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player got into win area");
        int enemyCount = LevelManager.instance.GetEnemyCount();
        int offSet = LevelManager.instance.GetEnemyOffset();

        if (collision.gameObject.tag == "Player" && enemyCount <= offSet)
        {
            LevelManager.instance.OnWin();
        }
    }
}
