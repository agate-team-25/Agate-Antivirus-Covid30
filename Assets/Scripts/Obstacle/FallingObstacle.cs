using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : Obstacle
{
    [Header("Player Detection Component")]
    // detection range of enemies to check player (range x and y)
    // note: range y only to below
    public float detectRangeX = 7;
    public float detectRangeY = 20;

    [Header("Fall Component")]
    // obstacle mass
    public float obstacleMass = 10;
    // delay before the object fall after detecting player
    public float fallDelay = 1;
    // delay before the object get destroyed
    public float destroyDelay = 2;

    // to save object already detected player or not, already fall or not and counter for delay
    private bool playerDetected;
    private bool hasFallen;
    private float counter;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerDetected = false;
        hasFallen = false;
        counter = fallDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // if player already detected nearby
        if (playerDetected)
        {
            fallDelay -= Time.deltaTime;
            if (fallDelay <= 0)
            {
                if (!hasFallen)
                {
                    // add rigidbody to make object fall
                    Rigidbody2D rigid = gameObject.AddComponent<Rigidbody2D>();
                    rigid.mass = obstacleMass;
                    hasFallen = true;
                }
            }
        }

        else
        {
            playerDetected = CheckIfPlayerNearby();
        }
    }

    private bool CheckIfPlayerNearby()
    {
        // return is player already destroyed
        if (player == null)
        {
            return false;
        }

        // get player position and enemy position
        Vector2 playerPos = player.transform.position;
        Vector2 obstaclePos = gameObject.transform.position;

        // get abs x range of player and obstacle
        float playerRangeX = Mathf.Abs(playerPos.x - obstaclePos.x);
        // get y range of player and obstacle
        float playerRangeY = obstaclePos.y - playerPos.y;

        // check if player is within enemy detection range (for y below only) 
        if (playerRangeX <= detectRangeX && playerRangeY > 0 && playerRangeY <= detectRangeY)
        {
            // if true, then player is nearby
            return true;
        }
        return false;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // set damage to 0 so player didnt get damaged anymore after touching it
        damage = 0;

        // destroy when hitting something
        // Add hitting sound effect and maybe animation here later
        // --HITTING ANIMATION AND SFX--
        StartCoroutine(DestroyObstacle());
    }

    IEnumerator DestroyObstacle()
    {
        yield return new WaitForSeconds(destroyDelay);
        // Add destroyed animation and sound effect here later
        // --DESTROYED ANIMATION AND SFX--
        Destroy(gameObject);
    }

    // to draw debug line
    public virtual void OnDrawGizmos()
    {
        // draw enemy detection range debug line
        Debug.DrawLine(transform.position, transform.position + (Vector3.left * detectRangeX), Color.yellow);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * detectRangeX), Color.yellow);
        Debug.DrawLine(transform.position - (Vector3.left * detectRangeX), transform.position - (Vector3.left * detectRangeX) + (Vector3.down * detectRangeY), Color.yellow);
        Debug.DrawLine(transform.position + (Vector3.left * detectRangeX), transform.position + (Vector3.left * detectRangeX) + (Vector3.down * detectRangeY), Color.yellow);
    }
}
