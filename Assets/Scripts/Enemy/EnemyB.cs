using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    #region variables
    [Header("Right/Left Movement Component")]
    // monster travelling range (counted from left to right or otherwise)
    public float travelRange = 7;

    [Header("Raycast Component")]
    // for raycast distance to detect object right and left
    public float raycastDistance = 0.75f;
    public float raycastWidth = 0.7f;
    public LayerMask environmentLayer;

    [Header("Shooting Component")]
    // shooting component
    public float shootDelay = 2;
    public float projectileTime = 5;
    public float projectileSpeed = 4;
    public GameObject bulletPrefab;
    public Transform firePoint;

    // status if player is nearby or not
    private bool playerNearby;

    // status if player is still alive or not
    private bool playerAlive;

    // to save the left and right limit of x position of enemy based on travel range
    private float leftX;
    private float rightX;

    // counter until enemy can shoot another bullet
    private float shootCounter;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // set to false initially so the enemy doesnt shoot
        playerNearby = false;

        // set x left and right limit of enemy
        leftX = transform.position.x - (travelRange / 2);
        rightX = transform.position.x + (travelRange / 2);

        // move immediately based on whichever direction enemy is facing
        MovingLeftOrRight();

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
    }

    private void FixedUpdate()
    {
        // call method to automatically change direction if collide or reaching the travel range limit
        CheckDirection();

        // decrease shoot counter every time even when player is not in range
        shootCounter -= Time.deltaTime;

        // check if there is player nearby and enemy still alive
        if (playerNearby && playerAlive && CheckIsAlive())
        {
            // call function to shoot at enemy
            Shoot();
        }
    }

    // method to check if enemy need to change direction or not, and if it need to then change direction automatically
    private void CheckDirection()
    {
        // get current x position
        float currentX = transform.position.x;

        if (facingLeft)
        {
            // raycasting above to check if enemy hit ceilling
            Vector3 raycastGap = new Vector3(0, raycastWidth / 2, 0);

            // raycast from the bottom, middle, and upper to cover a wide area
            RaycastHit2D hitBottom = Physics2D.Raycast(transform.position - raycastGap, Vector2.left, raycastDistance, environmentLayer);
            RaycastHit2D hitMiddle = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, environmentLayer);
            RaycastHit2D hitUpper = Physics2D.Raycast(transform.position + raycastGap, Vector2.left, raycastDistance, environmentLayer);

            // check if one of the raycast hit
            bool hit = (hitBottom || hitMiddle || hitUpper);

            // check if collide with ceiling or distance travelled already surpassing travel range
            if (hit || currentX <= leftX)
            {
                //Debug.Log("change direction from up to down");

                // if true, change direction to down
                FlipY();
                MovingLeftOrRight();
            }
        }

        else
        {
            // raycasting below to check if enemy hit floor
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, environmentLayer);

            // check if collide with floor or current y position already below original y position
            if (hit || currentX >= rightX)
            {
                //Debug.Log("change direction from down to up");

                // if true, change direction to down
                FlipY();
                MovingLeftOrRight();
            }
        }
    }

    // method to move enemy left or right based on enemy facing left or right
    private void MovingLeftOrRight()
    {
        //Debug.Log("Moving left/right");

        // direction based on enemy currently going up or down
        int direction = (facingLeft ? -1 : 1);
        EnemyRigidBody.velocity = new Vector2(speed * direction, 0);
    }

    private void Shoot()
    {
        // get player x position
        float playerX = GetPlayerPosition().x;
        // get enemy x position
        float currentX = transform.position.x;
        // check if enemy facing player using xnor expression
        bool facingPlayer = (facingLeft && (playerX <= currentX)) || (!facingLeft && !(playerX <= currentX));

        // only shoot if counter are on 0 and enemy is facing player
        if (shootCounter <= 0 && facingPlayer)
        {
            // instantiate projectile prefab
            GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // set projectile type based on enemy c and immediately launch it
            projectile.GetComponent<EnemyProjectile>().LaunchProjectile(projectileTime, projectileSpeed, type);

            // reset shoot counter
            shootCounter = shootDelay;
        }
    }

    // to draw debug line specific for Enemy B
    public override void OnDrawGizmos()
    {
        // call base function to draw detection range
        base.OnDrawGizmos();

        // draw line trajectory of enemy path based on travel range
        Debug.DrawLine(transform.position, transform.position + (Vector3.left * (travelRange / 2)), Color.cyan);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * (travelRange / 2)), Color.cyan);

        Vector3 bottomRay = transform.position + new Vector3(0, -(raycastWidth / 2) , 0);
        Vector3 upperRay = transform.position + new Vector3(0, raycastWidth / 2, 0);

        // draw raycast in the bottom
        Debug.DrawLine(bottomRay, bottomRay + (Vector3.right * raycastDistance), Color.blue);
        Debug.DrawLine(bottomRay, bottomRay + (Vector3.left * raycastDistance), Color.blue);

        // draw raycast in the middle
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * raycastDistance), Color.blue);
        Debug.DrawLine(transform.position, transform.position + (Vector3.left * raycastDistance), Color.blue);

        // draw raycast in the upper
        Debug.DrawLine(upperRay, upperRay + (Vector3.right * raycastDistance), Color.blue);
        Debug.DrawLine(upperRay, upperRay + (Vector3.left * raycastDistance), Color.blue);
    }
}
