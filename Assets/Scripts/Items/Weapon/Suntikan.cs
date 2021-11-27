using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suntikan : MonoBehaviour
{
    public Transform shootPoint;
    public SuntikanBullet suntikanBullet;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(suntikanBullet, shootPoint.position, shootPoint.rotation);
        }
    }
}