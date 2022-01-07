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

    [Header("Status Effect")]
    public GameObject bleed;
    public GameObject fever;

    [Header("Movement")]
    public float speed;

    [Header("Jump")]
    public float jumpForce;   

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Power Up")]
    public GameObject weapon1;
    public GameObject weapon2;

    [Header("Animation")]
    public Animator animator;

    public ItemType itemType;
    //public PlayerAnimationType animType = PlayerAnimationType.Common;
    //public State state = State.Idle;

    //boolean field
    private float health;
    private bool canJump = true;
    private bool isOnGround;
    private bool isFalling;
    private bool isfacingRight = true;
    private bool apdActivate = false;
    private bool isWalk;
    private bool enableInput;

    private float faceDirectionX = 0f;

    public bool isDead;
    public int powerUpLevel = 0;
    public string status = "Healthy";
    private Vector2 maxForce = new Vector2(0,1);
    private float apdTimer = 10f;

    Vector2 movement;
    Rigidbody2D playerRigidbody;
    UnityArmatureComponent armatureComponent;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        //Mendapatkan komponen Animator
        animator = GetComponent<Animator>();
        animator.SetBool("die", false);
        isDead = false;
        armatureComponent = GetComponent<UnityArmatureComponent>();

        //Mendapatkan komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();
        maxForce *= jumpForce*1.4f;
        Debug.Log(maxForce);

        //Assign health
        health = maxHealth;

        enableInput = true;

        weapon1.SetActive(false);
        weapon2.SetActive(false);
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
        
        #region move
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
                //Debug.Log("APD deactivate");
                apdActivate = false;
                if(powerUpLevel == 0)
                {
                    //Debug.Log("layer changed");
                    animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), 1f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Base Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("Desinfektan Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Desinfektan Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("Gun Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Gun Layer"), 0f);
                }

                else if(powerUpLevel == 1)
                {
                    animator.SetLayerWeight(animator.GetLayerIndex("Gun Layer"), 1f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Gun Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Base Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("Desinfektan Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Desinfektan Layer"), 0f);
                }

                else if (powerUpLevel == 2)
                {
                    animator.SetLayerWeight(animator.GetLayerIndex("Desinfektan Layer"), 1f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Desinfektan Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("Gun Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Gun Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), 0f);
                    animator.SetLayerWeight(animator.GetLayerIndex("APD Base Layer"), 0f);
                }
            }
        }        

        //Move(faceDirectionX);        
        #endregion
    }

    private void Update()
    {
        if (enableInput)
        {
            Movement();
        }

        else
        {
            playerRigidbody.velocity = new Vector2(0,0);
        }

        Move(faceDirectionX);
    }

    private void Movement()
    {
        //Mendapatkan nilai input horizontal (-1,0,1)
        faceDirectionX = Input.GetAxisRaw("Horizontal");
        Vector2 velocityVector = playerRigidbody.velocity;
        Vector2 jump = new Vector2(0, 1);

        //Jump input key
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround && canJump)
            {
                //velocityVector.y += jumpForce;
                playerRigidbody.AddForce(jump * jumpForce);
                Debug.Log(jump);
                isOnGround = false;
                FindObjectOfType<AudioManager>().PlaySound("Jump");
            }
        }

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) || (faceDirectionX != 0 && isOnGround) && !FindObjectOfType<AudioManager>().isPlaying("Walk"))
        {
            FindObjectOfType<AudioManager>().PlaySound("Walk");
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || (isOnGround && faceDirectionX == 0) || !isOnGround || health <= 0)
        {
            FindObjectOfType<AudioManager>().StopSound("Walk");
        }

        

        //Debug mental
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //  addForce();
        //}

        //if (velocityVector.y <= maxVelocity)
        //{
        //    playerRigidbody.velocity = velocityVector;
        //}

        animator.SetBool("isGround", isOnGround);
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
        animator.SetBool("isWalk", h != 0);
        playerRigidbody.velocity = new Vector2(moveBy, playerRigidbody.velocity.y);        
    }

    public void GetDamage(float damage, EnemyType type)
    {
        //Debug.Log("Player taken hit from " + type);
        if (apdActivate == false)
        {
            FindObjectOfType<AudioManager>().PlaySound("Hurt");
            
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
                isDead = true;
                StartCoroutine(Death());
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
            weapon2.SetActive(false);
            weapon1.SetActive(true);            
            animator.SetLayerWeight(animator.GetLayerIndex("Gun Layer"), 1f);
            animator.SetLayerWeight(animator.GetLayerIndex("Desinfektan Layer"), 0f);
        }

        if (powerUpLevel == 1)
        {            
            weapon1.SetActive(false);
            animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), 1f);
            animator.SetLayerWeight(animator.GetLayerIndex("Gun Layer"), 0f);
        }

        powerUpLevel -= 1;
    }

    public void PickUpItemEffect()
    {
        #region Strategy
        switch (itemType)
        {
            case ItemType.P3K:
                Cured();
                break;
            case ItemType.WeaponBox:
                WeaponBox();
                break;
            case ItemType.APDBox:
                Immune();
                break;
        }
        #endregion
    }

    public IEnumerator Death()
    {
        yield return new WaitForEndOfFrame();
        if (apdActivate)
        {
            yield return null;
        }

        canJump = false;

        //Debug.Log("Player death animation");
        //animator.SetBool("die", true);
        animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("APD Base Layer"), 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("Desinfektan Layer"), 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("APD Desinfektan Layer"), 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("Gun Layer"), 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("APD Gun Layer"), 0f);

        if (isOnGround)
        {
            enableInput = false;
            faceDirectionX = 0;
        }

        animator.SetBool("die", true);
        FindObjectOfType<AudioManager>().PlaySound("Die");        
        StartCoroutine(OnLose());
    }

    public IEnumerator OnLose()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        LevelManager.instance.OnLose();
    }

    public void Fever()
    {
        //Debug.Log("Player got fever status");
        speed = 4;
        status = "Fever";
        fever.SetActive(true);
        Invoke("Cured", 5);
    }

    public void Bleed()
    {
        FindObjectOfType<AudioManager>().PlaySound("Hurt");
        canJump = false;
        status = "Bleed";
        bleed.SetActive(true);
        Invoke("Cured", 5);
    }

    public void Cured()
    {
        speed = 8;
        canJump = true;
        fever.SetActive(false);
        bleed.SetActive(false);
        status = "Healthy";
    }

    private void WeaponBox()
    {
        if(powerUpLevel < 2)
        {
            powerUpLevel += 1;
        }        

        if (powerUpLevel == 1)
        {
            //Debug.Log("Power up to level " + powerUpLevel);
            if (apdActivate)
            {
                animator.SetLayerWeight(animator.GetLayerIndex("APD Gun Layer"), 1f);
            }
            else
            {
                animator.SetLayerWeight(animator.GetLayerIndex("Gun Layer"), 1f);
            }
            weapon1.SetActive(true);
        }
        else if(powerUpLevel == 2)
        {
            //Debug.Log("Power up to level " + powerUpLevel);
            if (apdActivate)
            {
                animator.SetLayerWeight(animator.GetLayerIndex("APD Desinfektan Layer"), 1f);
            }
            else
            {
                animator.SetLayerWeight(animator.GetLayerIndex("Desinfektan Layer"), 1f);
            }
            weapon1.SetActive(false);
            weapon2.SetActive(true);
        }
    }

    public void Immune()
    {
        Cured();
        apdActivate = true;
        if (powerUpLevel == 0)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("APD Base Layer"), 1f);
        }

        else if(powerUpLevel == 1)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("APD Gun Layer"), 1f);
        }

        else if(powerUpLevel == 2)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("APD Desinfektan Layer"), 1f);
        }               
    }

    public bool GetAPDStatus()
    {
        return apdActivate;
    }

    public float GetAPDTime()
    {
        return apdTimer;
    }
}

public enum ItemType
{
    P3K,
    WeaponBox,
    APDBox
}

public enum State
{
    Idle,
    Jump,
    Die,
    Walk
}
