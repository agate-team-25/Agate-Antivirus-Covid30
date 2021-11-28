using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just a dummy script for testing enemy
public class PlayerController : MonoBehaviour
{
    //Untuk menjadikan object singleton
    private static PlayerController _instance = null;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerController>();
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
