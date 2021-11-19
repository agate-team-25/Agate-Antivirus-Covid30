using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    #region variables
    [Header("Raycast Component")]
    // for raycast distance to detect object above and below
    public float raycastDistance = 0.75f;
    public LayerMask groundLayer;
    #endregion

    // Start is called before the first frame update
    //void Start(){}

    // Update is called once per frame
    //void Update(){}

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
