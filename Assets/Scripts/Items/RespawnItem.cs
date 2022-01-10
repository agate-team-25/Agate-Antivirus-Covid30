using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnItem : MonoBehaviour
{
    public Items obj;
    public float timer;
    public Items objectToRespawn;

    private Items item;
    private float respawnTime;    
    
    // Start is called before the first frame update
    void Start()
    {
        respawnTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        Respawn();
    }

    public void Respawn()
    {
        if(obj == null)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = respawnTime;
                item = Instantiate(objectToRespawn, transform.position, transform.rotation);
                obj = item;
                item.transform.parent = gameObject.transform;
            }
        }
    }
}
