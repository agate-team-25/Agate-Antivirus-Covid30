using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    #region variables
    [Header("Bullet RigidBody")]
    // rigidbody of bullet
    public Rigidbody2D projectileRigidBody;

    // projectile time before despawning and projectile speed
    private float projectileTime;
    private float projectileSpeed;

    // enemy type which shoots the projectile
    private EnemyType enemyType;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (projectileRigidBody == null)
        {
            projectileRigidBody = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    //void Update()
    //{}

    // to set up projectile attributes and immediately launch projectile
    public void LaunchProjectile(float time, float speed, EnemyType type)
    {
        //Debug.Log("Set projectile attributes: time = " + time + ", speed = " + speed + ", enemy type= " + type);
        projectileTime = time;
        projectileSpeed = speed;
        enemyType = type;

        projectileRigidBody.velocity = transform.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Projectile")
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
            }
        }
        
    }
}
