using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    #region variables
    [Header("Up/Down Movement Component")]
    // monster travelling range (counted from bottom to upper)
    public float travelRange = 10;

    [Header("Raycast Component")]
    // for raycast distance to detect object above and below
    public float raycastDistance = 0.75f;
    public LayerMask groundLayer;

    // status if player is nearby or not
    private bool playerNearby;

    // status is enemy is going down, if false then enemy is going up
    private bool goingDown;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // set to false initially so the enemy doesnt move
        playerNearby = false;

        // at the beginning, enemy is going down first
        goingDown = true;
        MovingUpOrDown();
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

    }
    
    // method to move enemy up or down based on goingDown status
    private void MovingUpOrDown()
    {
        int direction;
        if (goingDown)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        EnemyRigidBody.velocity = new Vector2(0, speed*direction);
    }

    // to draw debug line specific for Enemy C
    public override void OnDrawGizmos()
    {
        // call base function to draw detection range
        base.OnDrawGizmos();

        // draw object raycast debug line to above and below
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * raycastDistance), Color.blue);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * raycastDistance), Color.blue);
    }
}
