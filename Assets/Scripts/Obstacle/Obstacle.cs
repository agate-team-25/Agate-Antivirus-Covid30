using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    #region variables
    [Header("Obstacle Property")]
    public ObstacleStatus obstacleStatus;
    public bool isDestroyable;
    public float hitCount = 1;
    public float damage = 0;

    [Header("Player Reference")]
    // Player singleton object, assign manually only if player is not singleton
    public PlayerController player;
    #endregion

    [Header("Hidden Object")]
    // Enemy/Item that is hidden by the obstacle, null if there is no object
    // note: object need to be set to not active first
    public GameObject hiddenObj;

    // Start is called before the first frame update
    public virtual void Start()
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

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Obstacle hit a collider");

        // check if obstacle collide with player
        if (collision.gameObject.tag == "Player" && player != null)
        {
            //Debug.Log("obstacle hit the player");
            
            // check if obstacle can cause bleeding
            if (obstacleStatus == ObstacleStatus.Bleed)
            {
                player.Bleed();
            }

            // obstacle will hurt the player only if damage more than 0 
            if (damage > 0)
            {
                //Debug.Log("obstacle damage the player");
                player.GetDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Obstacle hit a trigger");

        // check if bullet/projectile trigger hit the obstacle
        if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "Bullet")
        {
            if (isDestroyable)
            {
                hitCount -= 1;
                if (hitCount <= 0)
                {
                    // Add destroyed animation and sound effect here later
                    // --DESTROYED ANIMATION AND SFX--
                    //Debug.Log("Object destroyed");
                    Destroy(gameObject);

                    // When object destroyed, will immediately spawn hidden enemy
                    if (hiddenObj != null)
                    {
                        hiddenObj.SetActive(true);
                    }
                }
            }
        }
    }
}

public enum ObstacleStatus
{
    None, Bleed
}
