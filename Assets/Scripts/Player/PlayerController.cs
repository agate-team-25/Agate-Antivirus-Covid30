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

    [Header("Power Up")]
    public Transform weaponPoint;
    public GameObject weapon1;
    public GameObject weapon2;

    public ItemType itemType;

    //boolean field
    private bool isJumping;
    private bool canJump = true;
    private bool isOnGround;
    private bool isFalling;
    private bool isfacingRight = true;

    private float faceDirectionX = 0f;

    public int powerUpLevel = 0;    
    private float maxVelocity;

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
        maxVelocity = playerRigidbody.velocity.y + jumpAccel;
    }

    private void FixedUpdate()
    {
        //Mendapatkan nilai input horizontal (-1,0,1)
        faceDirectionX = Input.GetAxisRaw("Horizontal");

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
        
        if (isJumping && canJump)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        if (velocityVector.y <= maxVelocity)
        {
            playerRigidbody.velocity = velocityVector;
        }

        if (faceDirectionX > 0 && !isfacingRight)
        {
            Flip();
        }
        else if (faceDirectionX < 0 && isfacingRight)
        {
            Flip();
        }

        Move(faceDirectionX);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround && canJump)
            {
                isJumping = true;                
            }
        }      
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isfacingRight = !isfacingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    public void Move(float h)
    {
        float moveBy = h * speed;
        playerRigidbody.velocity = new Vector2(moveBy, playerRigidbody.velocity.y);        
    }

    public void PickUpItemEffect()
    {
        #region Strategy
        switch (itemType)
        {
            case ItemType.P3K:
                break;
            case ItemType.WeaponBox:
                WeaponBox();
                break;
            case ItemType.APDBox:
                break;
        }
        #endregion
    }

    public void Death()
    {

    }

    public void Fever()
    {
        speed = 3;
    }

    public void Bleed()
    {
        canJump = false;
    }

    public void Cured()
    {
        speed = 5;
        canJump = true;
    }

    private void WeaponBox()
    {
        if (powerUpLevel == 0)
        {
            powerUpLevel = 1;
            var weapon = Instantiate(weapon1, weaponPoint.position, weaponPoint.rotation);
            weapon.transform.parent = gameObject.transform;
        }
        else if(powerUpLevel == 1)
        {
            powerUpLevel = 2;
            Destroy(weapon1);
            var weapon = Instantiate(weapon2, weaponPoint.position, weaponPoint.rotation);
            weapon.transform.parent = gameObject.transform;
        }
    }

    private void APDBox()
    {

    }
}

public enum ItemType
{
    P3K,
    WeaponBox,
    APDBox
}