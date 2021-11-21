using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    #region variables
    [Header("Up/Down Movement Component")]
    // monster travelling range (counted from bottom to upper)
    public float travelRange = 5;

    [Header("Raycast Component")]
    // for raycast distance to detect object above and below
    public float raycastDistance = 0.75f;
    public LayerMask environmentLayer;

    [Header("Shooting Component")]
    // shooting component
    public float shootDelay = 2;
    public float projectileTime = 5;
    public float projectileSpeed = 4;
    public GameObject bulletPrefab;
    public Transform firePoint;
    // max y distance of player and enemy for enemy to shoot
    public float shootYDistance = 0.5f;

    // status if player is nearby or not
    private bool playerNearby;

    // status is enemy is going up, if false then enemy is going down
    private bool goingUp;

    // to save the original y position of enemy before going up and down
    private float originalY;
    #endregion

    // counter until enemy can shoot another bullet
    private float shootCounter;

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
    }

    private void FixedUpdate()
    {
        // call method to automatically change direction if collide or reaching the travel range limit
        CheckDirection();

        // decrease shoot counter every time even when player is not in range
        shootCounter -= Time.deltaTime;

        // check if there is player nearby and enemy still alive
        if (playerNearby && CheckIsAlive())
        {
            // call function to shoot at enemy
            Shoot();
        }
    }

    // method to check if enemy need to change direction or not, and if it need to then change direction automatically
    private void CheckDirection()
    {
        // get current y position

        float currentY = transform.position.y;

        if (goingUp)
        {
            // raycasting above to check if enemy hit ceilling
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, environmentLayer);

            // to get distance travelled from original y in the bottom to current y
            float distanceTravelled = currentY - originalY;

            // check if collide with ceiling or distance travelled already surpassing travel range
            if (hit || distanceTravelled >= travelRange)
            {
                //Debug.Log("change direction from up to down");

                // if true, change direction to down
                goingUp = false;
                MovingUpOrDown();
            }
        }

        else
        {
            // raycasting below to check if enemy hit floor
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, environmentLayer);

            // check if collide with floor or current y position already below original y position
            if (hit || currentY <= originalY)
            {
                //Debug.Log("change direction from down to up");

                // if true, change direction to down
                goingUp = true;
                MovingUpOrDown();
            }
        }
    }
    
    // method to move enemy up or down based on goingDown status
    private void MovingUpOrDown()
    {
        // direction based on enemy currently going up or down
        int direction = (goingUp ? 1 : -1);
        EnemyRigidBody.velocity = new Vector2(0, speed*direction);
    }

    private void Shoot()
    {
        // get player y distance from enemy
        float playerYDistance = Mathf.Abs(GetPlayerPosition().y - transform.position.y);

        // only shoot if counter are on 0 and y distance is close
        if (shootCounter <= 0 && shootYDistance >= playerYDistance)
        {
            // instantiate projectile prefab
            GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // set projectile type based on enemy c and immediately launch it
            projectile.GetComponent<EnemyProjectile>().LaunchProjectile(projectileTime, projectileSpeed, type);

            // reset shoot counter
            shootCounter = shootDelay;
        }
    }

    // to draw debug line specific for Enemy C
    public override void OnDrawGizmos()
    {
        // call base function to draw detection range
        base.OnDrawGizmos();

        // draw line trajectory of enemy path based on travel range
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * travelRange), Color.cyan);

        // draw object raycast debug line to above and below
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * raycastDistance), Color.blue);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * raycastDistance), Color.blue);
    }
}
