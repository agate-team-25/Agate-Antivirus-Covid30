using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnItem : MonoBehaviour
{
    //private Items obj;

    public float timer;
    public Items objectToRespawn;

    private Items item;
    private float respawnTime;

    private bool itemSpawned;
    
    // Start is called before the first frame update
    void Start()
    {
        respawnTime = 0;
        itemSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!itemSpawned)
        {
            respawnTime -= Time.deltaTime;
            if (respawnTime <= 0)
            {
                Respawn();
            }
        }
    }

    private void FixedUpdate()
    {
        if (item == null)
        {
            itemSpawned = false;
        }
    }

    public void Respawn()
    {
        if(item == null)
        {
            item = Instantiate(objectToRespawn, transform.position, transform.rotation);
            item.transform.parent = gameObject.transform;
            //Debug.Log("Spawned");
            itemSpawned = true;
            respawnTime = timer;
        }
    }

    public void resetTimer()
    {
        respawnTime = 0;
    }

    public bool CheckItemAvaibility()
    {
        return itemSpawned;
    }
}
