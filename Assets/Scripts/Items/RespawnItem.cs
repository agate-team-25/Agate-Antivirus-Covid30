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
    
    // Start is called before the first frame update
    void Start()
    {
        respawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Respawn();
    }

    public void Respawn()
    {
        if(item == null)
        {
            respawnTime -= Time.deltaTime;
            if(respawnTime <= 0)
            {
                respawnTime = timer;
                item = Instantiate(objectToRespawn, transform.position, transform.rotation);
                item.transform.parent = gameObject.transform;
            }
        }
    }

    public void resetTimer()
    {
        respawnTime = 0;
    }
}
