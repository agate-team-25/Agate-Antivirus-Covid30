using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

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
    [Header("Health")]
    public float maxHealth;

    [Header("Movement")]
    public float speed;

    [Header("Jump")]
    public float jumpAccel;   

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Power Up")]
    public UnityEngine.Transform weaponPoint;
    public GameObject weapon1;
    public GameObject weapon2;

    [Header("Animation")]
    public Animator animator;

    public ItemType itemType;

    //boolean field
    private float health;
    private bool isJumping;
    private bool canJump = true;
    private bool isOnGround;
    private bool isFalling;
    private bool isfacingRight = true;
    private bool apdActivate = false;
    private GameObject weapon_1;
    private GameObject weapon_2;

    private float faceDirectionX = 0f;

    public int powerUpLevel = 0;    
    private float maxVelocity;
    private float apdTimer = 10f;

    Vector2 movement;
    Rigidbody2D playerRigidbody;
    UnityArmatureComponent armatureComponent;
    PolygonCollider2D apd;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        //Mendapatkan komponen Animator
        animator = GetComponent<Animator>();
        armatureComponent = GetComponent<UnityArmatureComponent>();

        //Mendapatkan komponen Polygon collider
        apd = GetComponent<PolygonCollider2D>();

        //Mendapatkan komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();
        maxVelocity = playerRigidbody.velocity.y + jumpAccel;

        //Assign health
        health = maxHealth;

        //Instantiate weapon 1
        weapon_1 = Instantiate(weapon1, weaponPoint.position, weaponPoint.rotation);
        weapon_1.transform.parent = gameObject.transform;

        //Instantiate weapon 2
        weapon_2 = Instantiate(weapon2, weaponPoint.position, weaponPoint.rotation);
        weapon_2.transform.parent = gameObject.transform;

        weapon_1.SetActive(false);
        weapon_2.SetActive(false);
    }

    private void FixedUpdate()
    {        
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

        #region jump
        //Jump handle
        Vector2 velocityVector = playerRigidbody.velocity;
        
        if (isJumping && canJump)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }
        #endregion

        #region move
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

        if (apdActivate)
        {
            apdTimer -= Time.deltaTime;

            //Debug.Log("" + counter);

            if (apdTimer <= 0)
            {
                apd.enabled = false;
            }
        }

        //armatureComponent.animation.Play("idle");

        Move(faceDirectionX);
        #endregion
    }

    private void Update()
    {
        //Mendapatkan nilai input horizontal (-1,0,1)
        faceDirectionX = Input.GetAxisRaw("Horizontal");

        //Jump input key
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

    public void GetDamage(float damage, EnemyType type)
    {
        //Debug.Log("Player taken hit from " + type);
        if (apdActivate == false)
        {
            if (powerUpLevel <= 0)
            {
                health -= damage;
            }

            else
            {
                PowerDown();
            }

            if (type == EnemyType.EnemyB || type == EnemyType.EnemyC)
            {
                Fever();
            }

            if (health <= 0)
            {
                Death();
            }
        }        
    }

    public void GetDamage(float damage)
    {
        GetDamage(damage, EnemyType.EnemyA);
    }

    public void PowerDown()
    {
        if (powerUpLevel == 2)
        {
            weapon_2.SetActive(false);
            weapon_1.SetActive(true);
        }

        if (powerUpLevel == 1)
        {
            weapon_1.SetActive(false);
        }

        powerUpLevel -= 1;
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
        Destroy(gameObject);
        LevelManager.instance.OnLose();
    }

    public void Fever()
    {
        //Debug.Log("Player got fever status");
        speed = 3;
    }

    public void Bleed()
    {
        canJump = false;
        Invoke("CureFromBleed", 5);
    }

    public void CureFromBleed()
    {
        canJump = true;
    }

    public void Cured()
    {
        speed = 5;
        canJump = true;
    }

    private void WeaponBox()
    {
        powerUpLevel += 1;

        if (powerUpLevel == 1)
        {
            //Debug.Log("Power up to level " + powerUpLevel);
            weapon_1.SetActive(true);
        }
        else if(powerUpLevel == 2)
        {
            //Debug.Log("Power up to level " + powerUpLevel);
            weapon_1.SetActive(false);
            weapon_2.SetActive(true);
        }
    }

    public void Immune()
    {
        apdActivate = true;
        apd.enabled = true;
    }
}

public enum ItemType
{
    P3K,
    WeaponBox,
    APDBox
}

public enum PlayerAnimationType
{
    Common,
    Suntikan,
    Desinfektan,
    APDCommon,
    APDSuntikan,
    APDDesinfektan
}