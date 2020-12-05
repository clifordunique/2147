using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder (-100)]
public class PlayerScript : MonoBehaviour
{
    GameManagerScript gameManagerScript;
    StressReceiver cameraStressReciever;
    PlayerMovement playerMovement;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    Material material;
    Color hurtColor;
    float currentFloppyCount;
    [HideInInspector] public float horizontal;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool crouchPressed;
    [HideInInspector] public bool firePressed;
    [HideInInspector] public bool isOnGround;
    [HideInInspector] public bool isOnGroundBuffer;
    [HideInInspector] public bool facingRight;
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool swipedRight;
    [HideInInspector] public bool swipedLeft;
    [HideInInspector] public int redMag, sweepCount, laserCount, shieldCount, missileCount, slowMoCount;
    [HideInInspector] public bool canMove = true, win;
    GameObject checkpointText;
    private int i;
    private float deathShakeMagnitude = 0.8f;
    private float fade;
    //To Decrement health when falling into acid.
    private float healthDecrementValue;
    //To Shake Camera while Dying.
    private bool isShaking;
    void Start()
    {
        facingRight = true;

        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        cameraStressReciever = GameObject.Find("Player Camera").GetComponent<StressReceiver>();

        checkpointText = GameObject.FindWithTag("Canvas").transform.Find("Checkpoint Text").gameObject;
        checkpointText.SetActive(false);

        hurtColor = new Color(1, 0.5f, 0.5f);

        redMag = 5;
        sweepCount = 5;
        laserCount = 10;
        shieldCount = 5;
        missileCount = 3;
        slowMoCount = 3;

        currentFloppyCount = 0;
    }
    
    public IEnumerator FlipSprite()
    {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        facingRight = !facingRight;
        transform.Rotate (0,180,0);
        canMove = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Platform"))
        {
            gameObject.transform.parent = col.gameObject.transform;
        }
        else if (col.CompareTag("WinDoor"))
        {
            win = true;
        }
        else if (col.CompareTag("Floppy"))
        {
            gameManagerScript.ChangeFloppyCount();
            col.gameObject.SetActive(false);
        }
        else if (col.CompareTag("Health"))
        {
            if (gameManagerScript.currentHealth != gameManagerScript.maxHealth)
            {
                i = 0;
                col.gameObject.SetActive(false);
                InvokeRepeating("IncreaseHealth", 0, 0.05f);
            }
        }
        else if (col.CompareTag("CheckPoint"))
        {
            gameManagerScript.lastCheckpoint.position = col.gameObject.transform.position;
            Destroy (col.gameObject);
            checkpointText.SetActive(true);
            Invoke("DisableCheckpointText", 1);
        }
        else if (col.CompareTag("Poison"))
        {
            playerMovement.rb.velocity = new Vector2(0,0);
            canMove = false;
            fade = 1;
            InvokeRepeating("AcidEffect", 0f, 0.01f);
        }
    }

    void AcidEffect()
    {
        if (!isShaking)
        {
            cameraStressReciever.InduceStress(deathShakeMagnitude);
            isShaking = true;
            healthDecrementValue = gameManagerScript.currentHealth / 100;
            Handheld.Vibrate();
        }
        fade -= 0.01f;
        material.SetFloat("_Fade", fade);

        gameManagerScript.ChangeHealth(healthDecrementValue);
        if (fade <= 0)
        {
            isShaking = false;
            material.SetFloat("_Fade", 1);
            CancelInvoke("AcidEffect");
        }
    }


    void DisableCheckpointText()
    {
        checkpointText.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Platform"))
        {
            gameObject.transform.SetParent(null);
        }
    }

    //Increase health on picking up health power up.
    //This Function is repeatedly called to make it look like the health is increasing gradually.
    //Health is increased by 30 hp.
    void IncreaseHealth()
    {
        gameManagerScript.SetHealth();
        if (gameManagerScript.currentHealth == gameManagerScript.maxHealth || i == 30)
        {
            CancelInvoke("IncreaseHealth");
        }
        gameManagerScript.currentHealth += 2;
        i ++;
    }
}
