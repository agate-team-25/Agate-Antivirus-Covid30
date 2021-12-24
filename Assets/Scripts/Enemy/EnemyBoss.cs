using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    #region variables

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

    // counter until enemy can shoot another bullet
    private float shootCounter;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // set to false initially so the enemy doesnt shoot
        playerNearby = false;

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
        // decrease shoot counter every time even when player is not in range
        shootCounter -= Time.deltaTime;

        // check if there is player nearby and enemy still alive
        if (playerNearby && playerAlive && CheckIsAlive())
        {
            // call function to shoot at enemy
            Shoot();
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

            // Add shooting sfx
            FindObjectOfType<AudioManager>().PlaySound("Enemy_Shoot");

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
