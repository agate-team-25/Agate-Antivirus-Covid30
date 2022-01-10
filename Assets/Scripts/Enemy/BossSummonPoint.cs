using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonPoint : MonoBehaviour
{
    public Collider2D collider;
    public int objInArea;


    private void Start()
    {
        objInArea = 0;
    }

    private void OnTriggerEnter()
    {
        objInArea += 1;
    }

    private void OnTriggerExit()
    {
        objInArea -= 1;
    }
}
