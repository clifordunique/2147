using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerScript playerScript;
    Animator animator;
    GameManagerScript gameManagerScript;
    [HideInInspector] public Rigidbody2D rb;
    RaycastHit2D wallHit, ledgeHit;
    float xVelocity;
    //attemptingGrab is false when starting the game. Turns true when a ledge is detected.
    //climbingLedge is true when animator play Ledge_Climb animation.
    private bool attemptingGrab, climbingLedge;
    [SerializeField] private float wallCheckDistance;
    [Header("Player Speed")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private Transform groundLeft, groundRight, wallCheck, ledgeCheck, climbMovePosition;
    private bool jumpBuffer;
    private float attemptingGrabBuffer;
    //tempX is to store the x position of the player when attemptingGrab is true.
    //It is checked with the player's current position before climbing.
    float tempX;
    float tempY;
    float isOnGroundBuffertime;
    bool changingBufferBool;
    bool jumpWasPressed;

    void Awake()
    {
        groundLeft = gameObject.transform.Find("GroundCheckLeft").GetComponent<Transform>();
        groundRight = gameObject.transform.Find("GroundCheckRight").GetComponent<Transform>();
        wallCheck = gameObject.transform.Find("WallCheck").GetComponent<Transform>();
        ledgeCheck = gameObject.transform.Find("LedgeCheck").GetComponent<Transform>();
        climbMovePosition = gameObject.transform.Find("ClimbMovePoint").GetComponent<Transform>();

        playerScript = GetComponent<PlayerScript>();
        animator = GetComponent<Animator>();
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PhysicsCheck();
    }

    void FixedUpdate()
    {
        if (playerScript.canMove && rb.bodyType != RigidbodyType2D.Kinematic)
        {
            Movement();
        }
    }
    void PhysicsCheck()
    {
        //Checking if player is on Ground.
        if (Physics2D.Raycast (groundLeft.position, Vector2.down, 0.3f, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Raycast (groundRight.position, Vector2.down, 0.3f, 1 << LayerMask.NameToLayer("Ground")))
        {
            playerScript.isOnGround = true;
            playerScript.isOnGroundBuffer = true;
            rb.gravityScale = 1;

            jumpWasPressed = false;
        }
        else
        {
            playerScript.isOnGround = false;
            if (!changingBufferBool)
            {
                changingBufferBool = true;
                Invoke ("ChangeIsOnGroundBuffer", 0.1f);
            }
        }
    }

    void ChangeIsOnGroundBuffer()
    {
        playerScript.isOnGroundBuffer = false;
        changingBufferBool = false;
    }

    void CheckForLedge()
    {
        if (playerScript.facingRight)
        {
            wallHit = Physics2D.Raycast (wallCheck.position, Vector2.right, wallCheckDistance, 1 << LayerMask.NameToLayer("Ground"));
            ledgeHit = Physics2D.Raycast (ledgeCheck.position, Vector2.right, wallCheckDistance, 1 << LayerMask.NameToLayer("Ground"));
        }
        else
        {
            wallHit = Physics2D.Raycast (wallCheck.position, Vector2.left, wallCheckDistance, 1 << LayerMask.NameToLayer("Ground"));
            ledgeHit = Physics2D.Raycast (ledgeCheck.position, Vector2.left, wallCheckDistance, 1 << LayerMask.NameToLayer("Ground"));
        }
    }


    void Movement()
    {
        //Flipping the sprite.
        if (playerScript.horizontal > 0 && !playerScript.facingRight || playerScript.horizontal < 0 && playerScript.facingRight)
        {
            StartCoroutine(playerScript.FlipSprite());
        }

        //Player moving.
        xVelocity = moveSpeed * playerScript.horizontal;
        rb.velocity = new Vector2 (xVelocity, rb.velocity.y);

        //Player falling.(increase gravityscale)
        if (!playerScript.isOnGround && rb.velocity.y < 20f && !climbingLedge)
        {
            rb.gravityScale = 5.5f;
        }

        //Player jumping.
        if ((playerScript.isOnGround || (playerScript.isOnGroundBuffer && !jumpWasPressed)) && !playerScript.isCrouching)
        {
            if (playerScript.jumpPressed || jumpBuffer)
            {
                transform.parent = null;

                jumpWasPressed = true;

                rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
            }
        }
        
        //jumpBuffer true for 0.4 seconds when the player is not on the ground.
        else if (!playerScript.isOnGround && playerScript.jumpPressed)
        {
            jumpBuffer = true;

            Invoke("ChangeBuffer", 0.4f);
        }

        //Checking if Player can grab a ledge.
        if (!climbingLedge)
        {
            CheckForLedge();

            //attemptingGrab is false when starting the game. It turns true when a ledge that can be grabbed is detected.
            //It turns true when a ledge is finished climbing.
            if (wallHit && !ledgeHit && !attemptingGrab)
            {
                attemptingGrab = true;
                //attemptinggrabBuffer is used to reset attemptingGrab bool to negative when certain amount of time passes and no ledge is climbed.
                attemptingGrabBuffer = Time.time;
                tempX = rb.position.x;
                tempY = rb.position.y;
            }
            if (attemptingGrab)
            {
                //Only climbs if the player has detected ledge just now and close to that detected position.
                if (Time.time - attemptingGrabBuffer < 0.25f && Mathf.Abs(rb.position.x - tempX) < 0.4 && Mathf.Abs(rb.position.y - tempY) < 0.4f)
                {
                    CheckForLedge();
                    //Climbing Ledge.
                    if (wallHit && ledgeHit && Mathf.Abs(playerScript.horizontal )> 0)
                    {
                        climbingLedge = true;
                        rb.velocity = new Vector2(0,0);
                        if (playerScript.facingRight)
                        {
                            transform.position = ledgeHit.point + new Vector2(0.3f,0);
                        }
                        else
                        {
                            transform.position = ledgeHit.point - new Vector2(0.3f,0);
                        }
                        rb.bodyType = RigidbodyType2D.Kinematic;

                        //Playing climb animation.
                        animator.Play("Ledge_Climb_Animation");
                    }

                }
                else
                {
                    attemptingGrab = false;
                }
            }
        }
    }
    void ChangeBuffer()
    {
        jumpBuffer = false;
    }

    //Function called at last frame of Ledge_Climb animation.
    void AfterClimbing()
    {
        climbingLedge = false;
        attemptingGrab = false;
        //animator.Play("Idle_without_Gun");
        animator.Play("Idle_without_Gun");
        transform.position = climbMovePosition.position;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}