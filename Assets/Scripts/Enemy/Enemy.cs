using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    #region variables
    // Enemy rigidbody, collider, and sprite renderer
    [Header("Object Component")]
    public Rigidbody2D EnemyRigidBody;
    public CircleCollider2D EnemyCollider;
    public SpriteRenderer EnemySprite;

    [Header("Player Reference")]
    // Player singleton object, assign manually only if player is not singleton
    // NOTE: change all PlayerControllerTest to PlayerController later
    public PlayerController player;

    [Header("Enemy Data")]
    // Enemy type
    public EnemyType type;
    // Enemy maximum health
    public int maxHealth = 1;
    // Enemy speed (not used for Enemy A)
    public int speed = 300;
    // Enemy current health
    private int health;
    // Status if enemy is still alive, might be used for something
    private bool isAlive;

    [Header("Player Detection Component")]
    // detection range of enemies to check player (range x and y)
    public float detectRangeX = 7;
    public float detectRangeY = 3;
    #endregion

    // Awake is called before Start
    void Awake()
    {
        // Get rigidbody, collider, and sprite renderer automatically if not filled
        if (EnemyRigidBody == null) {
            EnemyRigidBody = GetComponent<Rigidbody2D>();
        }
        
        if (EnemyCollider == null)
        {
            EnemyCollider = GetComponent<CircleCollider2D>();
        }

        if (EnemySprite == null)
        {
            EnemySprite = GetComponent<SpriteRenderer>();
        }

        // If player is singleton, can get the object automatically using code below
        if (player == null)
        {
            // Debug.Log("Player instance assigned automatically");
            player = PlayerController.Instance;
        }

        health = maxHealth;
        isAlive = true;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // if health is reduced to 0, call OnDeath() method
        if (health <= 0)
        {
            // Status enemy is already dead before the dead animation began
            isAlive = false;

            //Debug.Log("the enemy is dead");

            OnDeath();
        }
    }

    private void ReduceHealth(int damage)
    {
        health -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if enemy collide with a bullet / projectile
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Projectile")
        {
            // Add taking damage animation and sound effect here, if there any
            // --TAKING DAMAGE ANIMATION AND SFX--

            // if the bullet have damage, get it here
            int damage = 1;

            // Call reduce damage
            ReduceHealth(damage);
        }
    }

    // method to check if player are nearby, return bool value
    public bool CheckIfPlayerNearby()
    {
        // get player position and enemy position
        Vector2 playerPos = GetPlayerPosition();
        Vector2 enemyPos = gameObject.transform.position;

        // get x range of player and enemies
        float playerRangeX = Mathf.Abs(playerPos.x - enemyPos.x);
        float playerRangeY = Mathf.Abs(playerPos.y - enemyPos.y);

        // check if player is within enemy detection range
        if (playerRangeX <= detectRangeX && playerRangeY <= detectRangeY)
        {
            // if true, then player is nearby
            return true;
        }
        return false;
    }

    // just to get player position
    public Vector2 GetPlayerPosition()
    {
        return player.transform.position;
    }

    // just to check if the enemy is still alive
    public bool CheckIsAlive()
    {
        return isAlive;
    }

    // Called if enemy got killed. Need to be overridden for certain enemy types
    public virtual void OnDeath()
    {
        // Add death animation and sound effect here later (or maybe in the subclass override method)
        // --DEATH ANIMATION AND SFX--

        // Destroy game object in the end
        Destroy(this.gameObject);
    }

    // to draw debug line
    public virtual void OnDrawGizmos()
    {
        // draw enemy detection range debug line
        Debug.DrawLine(transform.position, transform.position + (Vector3.left * detectRangeX), Color.yellow);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * detectRangeX), Color.yellow);
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * detectRangeY), Color.yellow);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * detectRangeY), Color.yellow);
    }
}

public enum EnemyType
{
    EnemyA, EnemyB, EnemyC, EnemyBoss
}
