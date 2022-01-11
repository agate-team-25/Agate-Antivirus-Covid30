using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public bool isUp;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Debug.Log(isUp);
            transform.parent.GetComponent<Collider2D>().enabled = isUp;
        }
    }
}
