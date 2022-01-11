using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAreaBarrier : MonoBehaviour
{
    public GameObject killReqUi;
    public float uiUptime = 5;

    public bool checkpointActive;

    private float counter;
    private bool uiTriggered;

    private void Start()
    {
        counter = 0;
        uiTriggered = false;
        checkpointActive = false;
    }

    private void Update()
    {
        if (uiTriggered)
        {
            counter -= Time.deltaTime;
            if (counter <= 0 || !killReqUi.activeSelf)
            {
                killReqUi.SetActive(false);
                uiTriggered = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !checkpointActive && !uiTriggered)
        {
            killReqUi.SetActive(true);
            counter = uiUptime;
            uiTriggered = true;
        }
    }
}
