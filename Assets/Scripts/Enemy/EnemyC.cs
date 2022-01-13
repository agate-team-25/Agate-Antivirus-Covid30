using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    #region variables
    [Header("Up/Down Movement Component")]
    // monster travelling range (counted from bottom to upper)
    public float travelRange = 5;

    [Header("Shooting Component")]
    // shooting component
    public float shootDelay = 2;
    public float projectileTime = 5;
    public float projectileSpeed = 4;
    public GameObject bulletPrefab;
    public Transform firePoint;
    // max y distance of player and enemy for enemy to shoot
    public float shootYDistance = 0.3f;

    // status if player is nearby or not
    private bool playerNearby;

    // status if player is still alive or not
    private bool playerAlive;

    // status is enemy is going up, if false then enemy is going down
    private bool goingUp;

    // to save the original y position of enemy before going up and down
    private float originalY;

    // counter until enemy can shoot another bullet
    private float shootCounter;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // set to false initially so the enemy doesnt shoot
        playerNearby = false;

        // set original y position of enemy
        originalY = transform.position.y;

        // at the beginning, enemy is going up first
        goingUp = true;
        MovingUpOrDown();

        // set shoot counter to 0 to shoot immediately
        shootCounter = 0;
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

        // call function to check if player on the left or right, and flip accordingly
        if (playerNearby && playerAlive && CheckIsAlive())
        {
            CheckIfFacingPlayer();
        }
    }

    private void FixedUpdate()
    {
        // call method to automatically change direction if collide or reaching the travel range limit
        CheckDirection();

        // decrease shoot counter every time even when player is not in range
        shootCounter -= Time.deltaTime;

        // check if there is player nearby and enemy still alive
        if (shootCounter <= 0 && playerNearby && playerAlive && CheckIsAlive())
        {
            // get player y distance from enemy
            float playerYDistance = Mathf.Abs(GetPlayerPosition().y - transform.position.y);

            // check if y shoot distance less than player y distance from enemy
            if (shootYDistance >= playerYDistance)
            {
                // start coroutine to play shoot animation, and called Shoot() function right after coroutine finish executing
                StartCoroutine(ShootAnimation(() => { Shoot(); }));
                //Shoot();

                // reset shoot counter
                shootCounter = shootDelay;
            }
        }

        // Call function to move constantly to maintain speed
        MovingUpOrDown();
    }

    // method to check if enemy need to change direction or not, and if it need to then change direction automatically
    private void CheckDirection()
    {
        // get current y position
        float currentY = transform.position.y;

        if (goingUp)
        {
            // to get distance travelled from original y in the bottom to current y
            float distanceTravelled = currentY - originalY;

            // check if distance travelled already surpassing travel range
            if (distanceTravelled >= travelRange)
            {
                //Debug.Log("change direction from up to down");

                // if true, change direction to down
                goingUp = false;
            }
        }

        else
        {
            // check if current y position already below original y position
            if (currentY <= originalY)
            {
                //Debug.Log("change direction from down to up");

                // if true, change direction to down
                goingUp = true;
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // if collide with environment, then change direction (layer 6 currently refer to Environment layer)
        if (goingUp && (collision.gameObject.tag == "Obstacle" || collision.gameObject.layer == 6))
        {
            goingUp = false;
            //Debug.Log("goingUp :" + goingUp);
        }
    }

    // method to move enemy up or down based on goingDown status
    private void MovingUpOrDown()
    {
        // direction based on enemy currently going up or down
        int direction = (goingUp ? 1 : -1);
        EnemyRigidBody.velocity = new Vector2(0, speed*direction);
    }

    private IEnumerator ShootAnimation(System.Action onCompleted)
    {
        // play shooting animation
        animator.SetBool("Shooting", true);

        // wait until shooting animation finished
        yield return new WaitForSeconds(0.5f);

        // stop shooting animation
        animator.SetBool("Shooting", false);

        // invoke onCompleted to execute Shoot() function after
        onCompleted?.Invoke();
    }

    private void Shoot()
    {
        // instantiate projectile prefab
        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Add shooting sfx
        FindObjectOfType<AudioManager>().PlaySound("Enemy_Shoot");

        // set projectile type based on enemy c and immediately launch it
        projectile.GetComponent<EnemyProjectile>().LaunchProjectile(projectileTime, projectileSpeed, type);
    }

    private void CheckIfFacingPlayer()
    {
        // get player position and enemy position to decide direction to jump to
        Vector2 playerPos = GetPlayerPosition();
        Vector2 enemyPos = gameObject.transform.position;

        // check if player is on the left or right
        if (playerPos.x < enemyPos.x)
        {
            // flip horizontally if enemy currently facing right
            if (!facingLeft)
            {
                FlipY();
            }
        }
        else
        {
            // flip horizontally if enemy currently facing left
            if (facingLeft)
            {
                FlipY();
            }
        }
    }

    // to draw debug line specific for Enemy C
    public override void OnDrawGizmos()
    {
        // call base function to draw detection range
        base.OnDrawGizmos();

        // draw line trajectory of enemy path based on travel range
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * travelRange), Color.cyan);
    }
}
