using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {    
}

public abstract class Items : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {            
            PickUp();
        }
    }

    public abstract void PickUp();

    public abstract string GetName();
}