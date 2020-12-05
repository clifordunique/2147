using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color hurtColor;
    Transform groundCheckLeft, groundCheckRight;
    StressReceiver cameraStressReciever;
    Transform deathEffectPosition;
    [Header("GFX")]
    [SerializeField] GameObject[] deathSplash;
    [SerializeField] GameObject deathEffectParticle;
    [Header("Enemy Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float patrolRange;
    [Range(1,5)]
    [Header("Enemy AI (1-5)")]
    [SerializeField] private int difficulty;
    [HideInInspector] public float detectionDistance;
    [HideInInspector] public float alertDistance;
    [HideInInspector] public float defaultDetectionDistance;
    [HideInInspector] public float magCapacity;
    [HideInInspector] public float playerDetectionSpeed;
    [HideInInspector] public float bulletDamage;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool playerDetected;
    [HideInInspector] public bool shootPlayer;
    [HideInInspector] public float currentHealth;
    private float  maxHealth, startpos, endPos;
    private bool isOnGround, isPatrolling;
    [HideInInspector] public bool isDead;
    //For Changing the colour when hurt.
    //Canvas to show enemy health.
    [HideInInspector] public GameObject canvas;
    [SerializeField] private Slider healthSlider;
    private float shakeMagnitude = 0.3f;
    private int randomSplashEffect;
    
    void Awake()
    {
        SetEnemyProperties();
    }
    void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        groundCheckLeft = transform.Find("GroundCheckLeft");
        groundCheckRight = transform.Find("GroundCheckRight");
        canvas = transform.Find("Canvas").gameObject;
        
        hurtColor = new Color(1, 0, 0);

        currentHealth = maxHealth;
        
        startpos = transform.position.x - patrolRange;
        endPos = transform.position.x + patrolRange;

        HealthCheck();

        deathEffectPosition = transform.Find("DeathEffectPosition").GetComponent<Transform>();
        cameraStressReciever = GameObject.Find("Player Camera").GetComponent<StressReceiver>();
    }

    void SetEnemyProperties()
    {
        //Roboid
        if (difficulty == 1)
        {
            maxHealth = 50;
            bulletDamage = 20;
            healthSlider.maxValue = maxHealth;
        }

        //Soldier
        else if (difficulty == 2)
        {
            maxHealth = 60;
            bulletDamage = 23;
        }

        //Robo-Soldier
        else if (difficulty == 3)
        {
            maxHealth = 70;
            bulletDamage = 25;
        }

        //Exoskeleton
        else if (difficulty == 4)
        {
            maxHealth = 85;
            bulletDamage = 30;
        }

        //Spacenaut
        else if (difficulty == 5)
        {
            maxHealth = 100;
            bulletDamage = 35;
        }
        
        magCapacity = 3;
        playerDetectionSpeed = 0.01f;
        detectionDistance = defaultDetectionDistance = 25;
        alertDistance = 15;        
    }

    void FixedUpdate()
    {
        if (isOnGround && !playerDetected && !isDead)
        {
            MoveEnemy();
        }
    }

    void Update()
    {
        PhysicsCheck();

        //Enemy do not move if player is detected.
        if (playerDetected)
        {
            rb.velocity = new Vector2 (0,0);
            animator.SetBool("IsMoving", false);
        }
    }

    //Moving Enemy.
    void MoveEnemy()
    {
        if (facingRight && !isPatrolling)
        {
            animator.SetBool("IsMoving", true);
            rb.velocity = new Vector2 (moveSpeed, 0);
        }
        else if (!facingRight && !isPatrolling)
        {
            animator.SetBool("IsMoving", true);
            rb.velocity = new Vector2 (-moveSpeed, 0);
        }

        //Changing Enemy Direction.
        if (facingRight && transform.position.x > endPos && !isPatrolling)
            StartCoroutine(FlipSprite());
        if (!facingRight && transform.position.x < startpos && !isPatrolling)
            StartCoroutine(FlipSprite());
    }

    //Physics Check.
    void PhysicsCheck()
    {
        //Checking if Enemy is on Ground.
        if (Physics2D.Raycast (groundCheckLeft.position, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Raycast (groundCheckRight.position, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer("Ground")))
        {
            isOnGround = true;
            animator.SetBool("IsOnGround", true);
        }
        else
        {
            isOnGround = false;
            animator.SetBool("IsOnGround", false);
        }
    }

    void HealthCheck()
    {
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Instantiate (deathEffectParticle, deathEffectPosition.position, transform.rotation);
            randomSplashEffect = Random.Range(0,5);
            Instantiate (deathSplash[randomSplashEffect], transform.position, transform.rotation);
            isDead = true;
            animator.SetBool("IsMoving", false);
            rb.velocity = new Vector2(0,0);
            Destroy(gameObject);
            
            cameraStressReciever.InduceStress(shakeMagnitude);

            //Remove Rigidbody and canvas gameobjects
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.enabled = false;
            canvas.SetActive(false);
        }
    }

    public void ChangeHealth(int hurtValue)
    {
        currentHealth -= hurtValue;
        
        HealthCheck();
    }

    //Flipping Sprite.
    IEnumerator FlipSprite()
    {
        isPatrolling =  true;
        rb.velocity = new Vector2 (0,0);
        animator.SetBool("IsMoving", false);
        
        yield return new WaitForSeconds(2);
        
        isPatrolling = false;

        //Enemy changes direction if Player is not detected and if Enemy is not dead.
        if (!playerDetected && !isDead)
        {
            facingRight = !facingRight;
            transform.Rotate (0,180,0);
            canvas.transform.Rotate (0,180,0);
        }
    }

    IEnumerator ChangeEnemyColor()
    {
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds (0.2f);
        spriteRenderer.color = new Color(1,1,1);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
