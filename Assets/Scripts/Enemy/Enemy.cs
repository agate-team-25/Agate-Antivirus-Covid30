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
    public Collider2D EnemyCollider;
    public SpriteRenderer EnemySprite;

    [Header("Player Reference")]
    // Player singleton object, assign manually only if player is not singleton
    public PlayerController player;

    [Header("Enemy Data")]
    // Enemy type
    public EnemyType type;
    // Enemy maximum health
    public float maxHealth = 1f;
    // Enemy speed (not used for Enemy A)
    public int speed = 3;
    // How long until player get damaged when touching enemy
    public float damageTick = 1f;

    [Header("Direction")]
    // status is enemy is facing left, if false then enemy is facing right
    public bool facingLeft = true;

    [Header("Player Detection Component")]
    // detection range of enemies to check player (range x and y)
    public float detectRangeX = 7;
    public float detectRangeY = 3;

    [Header("Knockback Component")]
    // force of knockback to player when touching enemy
    public float knockbackForce = 100;

    [Header("Animation Component")]
    public Animator animator;
    public float deathDuration = 0f;

    // Enemy current health
    private float health;

    // Status if enemy is still alive, might be used for something
    private bool isAlive;

    // counter for damage tick
    private float tickCounter;

    // true if enemy is touching player
    private bool touchingPlayer;
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
            EnemyCollider = GetComponent<Collider2D>();
        }

        if (EnemySprite == null)
        {
            EnemySprite = GetComponent<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // If player is singleton, can get the object automatically using code below
        if (player == null)
        {
            // Debug.Log("Player instance assigned automatically");
            player = PlayerController.instance;
        }

        health = maxHealth;
        isAlive = true;
        tickCounter = damageTick;
        touchingPlayer = false;

        //check if enemy facing left or right. If facing right then flip horizontally
        if (!facingLeft)
        {
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnEnable()
    {
        GameObject[] otherEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in otherEnemies)
        {
            Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), EnemyCollider);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // checking is the enemy still alive moved to ReduceHealth() method so it wont get called multiple times

        // check if enemy touching player. If true, damage player continuously
        if (touchingPlayer)
        {
            tickCounter -= Time.deltaTime;
            if (tickCounter <= 0 && player != null && !player.isDead)
            {
                player.GetDamage(1, type);
                tickCounter = damageTick;
            }
        }
    }

    public void ReduceHealth(float damage)
    {
        if (type == EnemyType.EnemyBoss)
        {
            damage *= 1f;
        }

        health -= damage;

        //Debug.Log("Enemy " + type + " taken hit, remaining health: " + health);

        // Add taking damage animation and sound effect here, if there any
        // --TAKING DAMAGE ANIMATION AND SFX--
        FindObjectOfType<AudioManager>().PlaySound("Enemy_Taking_Damage");

        // if health is reduced to 0, call OnDeath() method
        if (health <= 0)
        {
            // Status enemy is already dead before the dead animation began
            isAlive = false;

            // Add death animation and sound effect here, or in the subclass override method
            // --DEATH ANIMATION AND SFX--
            if (type == EnemyType.EnemyBoss)
            {
                FindObjectOfType<AudioManager>().PlaySound("EnemyBoss_Death");
            }

            else
            {
                FindObjectOfType<AudioManager>().PlaySound("Enemy_Death");
            }

            //Debug.Log("the enemy is dead");

            // play death animation if the enemy have one
            animator.SetBool("Alive", false);

            // call OnDeath() with delay based on enemy death animation duration
            Invoke("OnDeath", deathDuration);
            //OnDeath();
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float h)
    {
        health = h;
    }

    // method to check if player are nearby, return bool value
    public bool CheckIfPlayerNearby()
    {
        // return is player already destroyed
        if (player == null)
        {
            return false;
        }

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
        if (player != null)
        {
            return player.transform.position;
        }

        // return enemy own position if player is destroyed so no error occured
        return transform.position;
    }

    // to check if player is still alive
    public bool CheckPlayerIsAlive()
    {
        if (player != null)
        {
            // if player still exist, return status is player active or not
            return player.gameObject.activeSelf;
        }

        // if player is null, then player is already destroyed (dead)
        return false;
    }

    // just to check if the enemy is still alive
    public bool CheckIsAlive()
    {
        return isAlive;
    }

    // Called if enemy got killed. Need to be overridden for certain enemy types
    public virtual void OnDeath()
    {
        // Destroy game object in the end
        Destroy(this.gameObject);
    }

    // To flip enemy object horizontally
    public void FlipY()
    {
        facingLeft = !facingLeft;
        transform.Rotate(0f, 180f, 0f);

        // source: https://youtu.be/wkKsl1Mfp5M
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player != null && !player.isDead)
            {
                player.GetDamage(1, type);
                tickCounter = damageTick;
                touchingPlayer = true;
            }
            
            // Notes: knockback belum bisa diimplementasikan karena autoreset velocity dari player
            // get knockback direction
            //Vector2 knockbackDirection = player.transform.position - transform.position;

            // give knockback to player when touching enemy
            //Debug.Log("Knockback");
            //player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce);
        }

        // NOTES: Only for testing enemy taking damage from bullet, call ReduceHealth from the bullet object instead, remove when finished
        //if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Projectile")
        //{
        //    // Call reduce damage
        //    ReduceHealth(1);
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingPlayer = false;
        }
    }

    // to draw debug line
    public virtual void OnDrawGizmos()
    {
        // draw enemy detection range debug line
        Debug.DrawLine(transform.position, transform.position + (Vector3.left * detectRangeX), Color.yellow);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * detectRangeX), Color.yellow);
        Debug.DrawLine(transform.position - (Vector3.left * detectRangeX), transform.position - (Vector3.left * detectRangeX) + (Vector3.up * detectRangeY), Color.yellow);
        Debug.DrawLine(transform.position - (Vector3.left * detectRangeX), transform.position - (Vector3.left * detectRangeX) + (Vector3.down * detectRangeY), Color.yellow);
        Debug.DrawLine(transform.position + (Vector3.left * detectRangeX), transform.position + (Vector3.left * detectRangeX) + (Vector3.up * detectRangeY), Color.yellow);
        Debug.DrawLine(transform.position + (Vector3.left * detectRangeX), transform.position + (Vector3.left * detectRangeX) + (Vector3.down * detectRangeY), Color.yellow);
    }
}

public enum EnemyType
{
    EnemyA, EnemyB, EnemyC, EnemyBoss
}
