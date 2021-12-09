using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : Enemy
{
    #region variables
    [Header("Jump Component")]
    // jump force (x, y) for enemy when jumping
    public float jumpForceX = 250;
    public float jumpForceY = 400;
    // jump delay after landing on the ground
    public float jumpDelay = 1;

    [Header("Raycast Component")]
    // for raycast distance to the ground and ground layer to detect
    public float groundRaycastDistance = 0.48f;
    public LayerMask environmentLayer;

    [Header("Explosion Component")]
    // radius of explosion
    public float explosionRadius = 3;
    // force of explosion
    public float explosionForce = 200;
    // delay before explosion
    public float explosionDelay = 3;

    // jump delay after landing on the ground
    private float delayCounter;

    // status if player is nearby or not
    private bool playerNearby;

    // status if player is still alive or not
    private bool playerAlive;

    // status if enemy is on the ground or not
    private bool onGround;

    // status if enemy has exploded or not
    private bool hasExploded;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // set to false initially so the enemy doesnt move
        playerNearby = false;

        onGround = true;
        hasExploded = false;

        // delay counter 0 initially so enemy immediately jump if player nearby
        delayCounter = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        // call superclass Update method
        base.Update();

        // call function to check if player is nearby
        playerNearby = CheckIfPlayerNearby();

        // call function to check if player is alive
        playerAlive = CheckPlayerIsAlive();
    }

    private void FixedUpdate()
    {
        // check if there is player nearby and enemy still alive
        if (playerNearby && CheckIsAlive())
        {
            // call function to check if enemy is on ground
            CheckOnGroud();

            // if player nearby and enemy currently on ground, enemy will move to the player direction by jumping if player is alive
            if (onGround && playerAlive)
            {
                Jump();
            }
        }
    }

    // method to check if enemy on the ground or air
    private void CheckOnGroud()
    {
        // raycasting below to check if enemy is on ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, environmentLayer);

        // if the raycast hit the ground and enemy already stay still, then the enemy is on the ground
        if (hit && EnemyRigidBody.velocity.x == 0 && EnemyRigidBody.velocity.y == 0)
        {
            // Debug.Log("Hit the ground");

            // Add idle animation after jumping here later
            // --IDLE ANIMATION--

            onGround = true;
        }

        else
        {
            // Debug.Log("In the air");
            onGround = false;
        }
    }

    // method to jump
    private void Jump()
    {
        if (delayCounter > 0)
        {
            delayCounter -= Time.deltaTime;
            return;
        }

        // get player position and enemy position to decide direction to jump to
        Vector2 playerPos = GetPlayerPosition();
        Vector2 enemyPos = gameObject.transform.position;

        // to keep direction to jump (left = -1, right = 1)
        int direction;

        // check if player is on the left or right
        if (playerPos.x < enemyPos.x)
        {
            // if player is on the left, then change direction to -1
            direction = -1;

            // flip horizontally if enemy currently facing right
            if (!facingLeft)
            {
                FlipY();
            }
        }
        else
        {
            // if player is on the right, then change direction to 1
            direction = 1;

            // flip horizontally if enemy currently facing left
            if (facingLeft)
            {
                FlipY();
            }
        }

        Vector2 jumpForce = new Vector2();
        jumpForce.x = jumpForceX * direction;
        jumpForce.y = jumpForceY;

        // Add jump animation and sound effect here later
        // --JUMP ANIMATION AND SFX--
        FindObjectOfType<AudioManager>().PlaySound("EnemyA_Jump");

        // add force to rigidbody to jump
        EnemyRigidBody.AddForce(jumpForce);

        // after jumping, reset delay counter
        delayCounter = jumpDelay;
    }

    // override superclass OnDeath() method to explode before dying
    public override void OnDeath()
    {
        // Add death animation and sound effect (before exploding) here later
        // --DEATH ANIMATION AND SFX--

        // call coroutine function to explode if enemy hasnt exploded yet
        if (!hasExploded)
        {
            // start coroutine to explode, and called superclass onDeath() function right after coroutine finish executing
            StartCoroutine(Explode(explosionDelay, () => { base.OnDeath(); }));
            
            // so the coroutine only called once
            hasExploded = true;
        }
        //StopCoroutine("Explode");
    }

    // coroutine method to explode
    private IEnumerator Explode(float time, System.Action onCompleted)
    {
        //Debug.Log("Explode Coroutine called");

        // delay before explosion occur
        yield return new WaitForSeconds(explosionDelay);

        // Maybe add exploding animation and sound effect here
        // --EXPLODE ANIMATION AND SFX--
        FindObjectOfType<AudioManager>().PlaySound("EnemyA_Explode");

        // get all of the object in the explosion radius
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // for loop each objects inside radius
        foreach (Collider2D obj in objects)
        {
            // get rigidbody of object
            Rigidbody2D objRigid =  obj.GetComponent<Rigidbody2D>();
            
            // only object who has rigidbody will be affected, except enemy
            if (objRigid != null && obj.gameObject.tag != "Enemy")
            {
                // get impact direction from player and enemy position
                Vector2 impactDirection = obj.transform.position - transform.position;
                float explosionDistance = impactDirection.magnitude;

                //Debug.Log("explosion distance: "+explosionDistance);
                //Debug.Log("explosion direction: " + impactDirection);
                //Debug.Log("explosion direction normalized: "+impactDirection/explosionDistance);

                // to calculate impact base power base on player distance from the enemy
                impactDirection /= explosionDistance;
                float forceCalculation = explosionRadius*1.2f - explosionDistance;

                // apply force to the objects with the impact calculation times explosion force
                //obj.GetComponent<Rigidbody2D>().AddForce(impactDirection * explosionForce);

                obj.GetComponent<Rigidbody2D>().AddForce(impactDirection * forceCalculation * explosionForce);

                // check if object is player
                if (obj.gameObject.tag == "Player")
                {
                    // also add damage to player here later
                    // --ADD DAMAGE TO PLAYER--
                    player.GetDamage(1);
                }

            }
        }
        // source: https://youtu.be/k4hr7-7ysCY

        // invoke onCompleted to execute superclass onDeath()
        onCompleted?.Invoke();
    }

    // to draw debug line specific for Enemy A
    public override void OnDrawGizmos()
    {
        // draw explosion area based on radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        // call base function to draw detection range
        base.OnDrawGizmos();

        // draw ground raycast debug line
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.blue);
    }
}
