using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region singleton
    private static PlayerController _instance = null;
    public static PlayerController instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<PlayerController>();
            }

            return _instance;
        }        
    }
    #endregion

    #region variables
    [Header("Movement")]
    public float speed;

    [Header("Jump")]
    public float jumpAccel;    

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    public ItemType itemType;

    //boolean field
    private bool isJumping;
    private bool isOnGround;
    private bool isFalling;
    private bool isFlip;

    public int powerUpLevel = 0;

    Vector2 movement;
    Rigidbody2D playerRigidbody;
    Animator anim;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        //Mendapatkan komponen Animator
        anim = GetComponent<Animator>();

        //Mendapatkan komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //Mendapatkan nilai input horizontal (-1,0,1)
        float h = Input.GetAxisRaw("Horizontal");

        // raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if (hit)
        {
            if (!isOnGround)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        Vector2 velocityVector = playerRigidbody.velocity;
        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        playerRigidbody.velocity = velocityVector;

        Move(h);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                isJumping = true;                
            }
        }
    }

    public void Move(float h)
    {
        float moveBy = h * speed;
        playerRigidbody.velocity = new Vector2(moveBy, playerRigidbody.velocity.y);
    }

    private void PickUpItemEffect()
    {
        #region Strategy
        switch (itemType)
        {
            case ItemType.P3K:
                break;
            case ItemType.WeaponBox:
                break;
            case ItemType.APDBox:
                break;
        }
        #endregion
    }

    public void Picked()
    {
        
    }
}

public enum ItemType
{
    P3K,
    WeaponBox,
    APDBox
}

public enum PowerUps
{
    Level1,
    Level2,
    APD
}