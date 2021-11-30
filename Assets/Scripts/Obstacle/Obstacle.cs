using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    #region variables
    [Header("Obstacle Property")]
    public ObstacleStatus obstacleStatus;
    public bool isDestroyable;

    [Header("Player Reference")]
    // Player singleton object, assign manually only if player is not singleton
    public PlayerController player;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // If player is singleton, can get the object automatically using code below
        if (player == null)
        {
            // Debug.Log("Player instance assigned automatically");
            player = PlayerController.instance;
        }
    }

    // Update is called once per frame
    //void Update()
    //{}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Obstacle hit a collider");

        // check if obstacle collide with player
        if (collision.gameObject.tag == "Player" && player != null)
        {
            // check if obstacle can cause bleeding
            if (obstacleStatus == ObstacleStatus.Bleed)
            {
                player.Bleed();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Obstacle hit a trigger");

        // check if bullet/projectile trigger hit the obstacle
        if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "Bullet")
        {
            if (isDestroyable)
            {
                // Add destroyed animation and sound effect here later
                // --DESTROYED ANIMATION AND SFX--
                Debug.Log("Object destroyed");
                Destroy(gameObject);
            }
        }
    }
}

public enum ObstacleStatus
{
    None, Bleed
}
