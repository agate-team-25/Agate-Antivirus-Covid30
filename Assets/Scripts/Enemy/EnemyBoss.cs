using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    #region variables

    [Header("Raycast Component")]
    // for raycast distance to detect object right and left
    //public float raycastDistance = 0.75f;
    public float groundRaycastWidth = 0.7f;
    public float groundRaycastDistance = 0.48f;
    //public float sideRaycastDistance = 0.38f;
    public LayerMask environmentLayer;

    [Header("Shooting Component")]
    // shooting component
    public float shootDelay = 2;
    public float projectileTime = 5;
    public float projectileSpeed = 4;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Boss Action")]
    // at how much health does the enemy began taking some action, and other attributes
    public float jumpingHealth = 70;
    public float summonHealth = 50;
    public int maxSummon = 5;
    public Enemy summon1;
    public Enemy summon2;
    public float jumpDelay = 1;
    public float summonDelay = 10;
    public float jumpForceX = 250;
    public float jumpForceY = 400;
    public Transform summonPoint1;
    public Transform summonPoint2;

    // status if player is nearby or not
    private bool playerNearby;

    // status if player is still alive or not
    private bool playerAlive;

    // counter until enemy can shoot another bullet
    private float shootCounter;

    // enemy movement type based on health
    private bool moveByJumping;

    // enemy can summon monster based on health
    private bool summonMonster;

    // summon enemy counter
    private float summonCounter;

    // List of monster summoned
    private List<Enemy> SummonedMonsters;

    // jump counter
    private float jumpCounter;

    private bool onGround;

    private Vector3 spawnPosition;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // set to false initially so the enemy doesnt shoot
        playerNearby = false;

        // set shoot counter to 0 to shoot immediately
        shootCounter = 0;

        moveByJumping = false;
        summonMonster = false;
        jumpCounter = 0;
        summonCounter = 0;
        onGround = true;

        spawnPosition = transform.position;

        SummonedMonsters = new List<Enemy>();

        if (jumpingHealth > maxHealth)
        {
            jumpingHealth = maxHealth;
        }

        if (summonHealth > maxHealth)
        {
            summonHealth = maxHealth;
        }
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

        CheckMoving();
        CheckOnGroud();

        UpdateSummonCount();

        // update phase based on health
        float health = GetHealth();
        moveByJumping = (health <= jumpingHealth);
        summonMonster = (health <= summonHealth);
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

            //CheckOnGroud();

            if (onGround)
            {
                FacePlayerDirection();
            }

            if (!moveByJumping)
            {
                //Debug.Log("Enemy boss move by walking");
                MovingLeftOrRight();
            }

            else
            {
                //Debug.Log("Enemy boss move by jumping");
                if (onGround)
                {
                    Jump();
                }
            }

            if (summonMonster)
            {
                //Debug.Log("Enemy boss summon a monster");
                SummonEnemy();
            }
        }
    }

    private void CheckMoving()
    {
        if (moveByJumping)
        {
            // if already changed phase to jumping, walking animation wont ever play anymore
            animator.SetBool("Moving", false);
        }

        else if (EnemyRigidBody.velocity.magnitude > 0)
        {
            animator.SetBool("Moving", true);
        }

        else
        {
            animator.SetBool("Moving", false);
        }
    }

    // method to check if enemy on the ground or air
    private void CheckOnGroud()
    {
        // get the gap between left/right to middle raycast
        Vector3 raycastGap = new Vector3(groundRaycastWidth / 2, 0, 0);

        // raycasting below to check if enemy is on ground
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position - raycastGap, Vector2.down, groundRaycastDistance, environmentLayer);
        RaycastHit2D hitMiddle = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, environmentLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + raycastGap, Vector2.down, groundRaycastDistance, environmentLayer);

        // check if one of the raycast hit
        bool hit = (hitLeft || hitMiddle || hitRight);

        // if the raycast hit the ground and enemy already stay still, then the enemy is on the ground
        if (hit && Mathf.Abs(EnemyRigidBody.velocity.x) <= 0.001f && Mathf.Abs(EnemyRigidBody.velocity.y) <= 0.001f)
        {
            //Debug.Log("Hit the ground");

            // Add idle animation after jumping here later
            // --IDLE ANIMATION--

            onGround = true;
        }

        else if (hit && !moveByJumping)
        {
            //Debug.Log("Hit the ground");

            // Add idle animation after jumping here later
            // --IDLE ANIMATION--

            onGround = true;
        }

        else
        {
            //Debug.Log("In the air");
            onGround = false;
        }

        // set jumping animation based on onground status
        animator.SetBool("OnGround", onGround);
    }

    // method to move enemy left or right based on enemy facing left or right
    private void MovingLeftOrRight()
    {
        //Debug.Log("Moving left/right");

        // direction based on enemy currently going up or down
        int direction = (facingLeft ? -1 : 1);
        float y = EnemyRigidBody.velocity.y;
        EnemyRigidBody.velocity = new Vector2(speed * direction, y);
    }

    // method to jump
    private void Jump()
    {
        if (jumpCounter > 0)
        {
            jumpCounter -= Time.deltaTime;
            return;
        }

        // get player position and enemy position to decide direction to jump to
        Vector2 playerPos = GetPlayerPosition();
        Vector2 enemyPos = gameObject.transform.position;

        // to keep direction to jump (left = -1, right = 1)
        int direction = (facingLeft ? -1 : 1);

        Vector2 jumpForce = new Vector2();
        jumpForce.x = jumpForceX * direction;
        jumpForce.y = jumpForceY;

        // Add jump animation and sound effect here later
        // --JUMP ANIMATION AND SFX--
        FindObjectOfType<AudioManager>().PlaySound("EnemyA_Jump");

        // add force to rigidbody to jump
        EnemyRigidBody.AddForce(jumpForce);

        // after jumping, reset delay counter
        jumpCounter = jumpDelay;
    }

    private void FacePlayerDirection()
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

    private void UpdateSummonCount()
    {
        //foreach (Enemy summon in SummonedMonsters)
        //{
        //    if (summon == null)
        //    {
        //        SummonedMonsters.Remove(summon);
        //    }
        //}

        for (int i = 0; i < SummonedMonsters.Count; i++)
        {
            if (SummonedMonsters[i] == null)
            {
                SummonedMonsters.RemoveAt(i);
            }
        }
    }

    private void SummonEnemy()
    {
        summonCounter -= Time.deltaTime;

        if (summonCounter > 0 || !onGround || SummonedMonsters.Count >= maxSummon)
        {
            return;
        }

        int type = Random.Range(0, 2);

        if (type == 0)
        {
            BossSummonPoint summonPoint = summonPoint1.GetComponent<BossSummonPoint>();

            if (summonPoint.objInArea > 0)
            {
                return;
            }

            //Debug.Log("Boss enemy summon enemy type A");
            Enemy summon = Instantiate(summon1, summonPoint1.position, summonPoint1.rotation);
            SummonedMonsters.Add(summon);
        }

        else
        {

            BossSummonPoint summonPoint = summonPoint2.GetComponent<BossSummonPoint>();

            if (summonPoint.objInArea > 0)
            {
                return;
            }

            //Debug.Log("Boss enemy summon enemy type B");
            Enemy summon = Instantiate(summon2, summonPoint2.position, summonPoint2.rotation);
            summon.FlipY();
            SummonedMonsters.Add(summon);
        }

        summonCounter = summonDelay;
    }

    public void ResetEnemyState()
    {
        if (GetHealth() <= 0)
        {
            return;
        }

        SetHealth(maxHealth);
        DestroySummonedEnemies();
        transform.position = spawnPosition;
    }

    public override void OnDeath()
    {
        DestroySummonedEnemies();
        base.OnDeath();
    }

    private void DestroySummonedEnemies()
    {
        foreach (Enemy summon in SummonedMonsters)
        {
            if (summon != null)
            {
                Destroy(summon.gameObject);
            }
        }
    }

    // to draw debug line specific for Enemy B
    public override void OnDrawGizmos()
    {
        // call base function to draw detection range
        base.OnDrawGizmos();

        //Vector3 bottomRay = transform.position + new Vector3(0, -(raycastWidth / 2) , 0);
        //Vector3 upperRay = transform.position + new Vector3(0, raycastWidth / 2, 0);

        //// draw raycast in the bottom
        //Debug.DrawLine(bottomRay, bottomRay + (Vector3.right * raycastDistance), Color.blue);
        //Debug.DrawLine(bottomRay, bottomRay + (Vector3.left * raycastDistance), Color.blue);

        //// draw raycast in the middle
        //Debug.DrawLine(transform.position, transform.position + (Vector3.right * raycastDistance), Color.blue);
        //Debug.DrawLine(transform.position, transform.position + (Vector3.left * raycastDistance), Color.blue);

        //// draw raycast in the upper
        //Debug.DrawLine(upperRay, upperRay + (Vector3.right * raycastDistance), Color.blue);
        //Debug.DrawLine(upperRay, upperRay + (Vector3.left * raycastDistance), Color.blue);

        Vector3 leftRay = transform.position + new Vector3(-(groundRaycastWidth / 2), 0, 0);
        Vector3 rightRay = transform.position + new Vector3(groundRaycastWidth / 2, 0, 0);

        // draw left ground raycast debug line
        Debug.DrawLine(leftRay, leftRay + (Vector3.down * groundRaycastDistance), Color.blue);

        // draw middle ground raycast debug line
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.blue);

        // draw right ground raycast debug line
        Debug.DrawLine(rightRay, rightRay + (Vector3.down * groundRaycastDistance), Color.blue);
    }
}
