using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    #region variables
    [Header("Bullet RigidBody")]
    // rigidbody of bullet
    public Rigidbody2D projectileRigidBody;

    [Header("Player Reference")]
    // Player singleton object, assign manually only if player is not singleton
    public PlayerController player;

    // projectile time before despawning and projectile speed
    private float projectileTime;
    private float projectileSpeed;

    // enemy type which shoots the projectile
    private EnemyType enemyType;

    // counter for projectile lifespan
    private float counter;

    // status if projectile is launched
    private bool isLaunched;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (projectileRigidBody == null)
        {
            projectileRigidBody = GetComponent<Rigidbody2D>();
        }

        // If player is singleton, can get the object automatically using code below
        if (player == null)
        {
            // Debug.Log("Player instance assigned automatically");
            player = PlayerController.instance;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log("uopdaeeee"+isLaunched);
        // check if the projectile already launched
        if (isLaunched)
        {
            // decrease counter
            counter -= Time.deltaTime;

            //Debug.Log("" + counter);
            
            if (counter <= 0)
            {
                // destroy object if counter reach zero
                Destroy(this.gameObject);
            }
        }
    }

    // to set up projectile attributes and immediately launch projectile
    public void LaunchProjectile(float time, float speed, EnemyType type)
    {
        //Debug.Log("Set projectile attributes: time = " + time + ", speed = " + speed + ", enemy type= " + type);
        projectileTime = time;
        projectileSpeed = speed;
        enemyType = type;

        // launch projectile
        projectileRigidBody.velocity = transform.right * projectileSpeed;
        // set counter and status after launch
        counter = projectileTime;
        isLaunched = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Projectile" && collision.tag != "Non Physical" && collision.tag != "Enemy")
        {
            //Debug.Log("enemy projectile hit a " + collision.name);

            // Add projectile hit animation and sound effect here
            // --PROJECTILE HIT ANIMATION AND SFX--
            Destroy(gameObject);

            // get PlayerController script from the collision object
            PlayerController player = collision.GetComponent<PlayerController>();
            // if player is null, then that means the collision object is not a player
            if (player != null)
            {
                // Call PlayerController method to give damage to player here
                player.GetDamage(1, enemyType);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Non Physical" && collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
