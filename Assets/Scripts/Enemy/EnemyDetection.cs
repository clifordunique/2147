using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetection : MonoBehaviour
{
    Camera cam;
    private EnemyScript enemyScript;
    private Transform firePoint;
    private Slider playerDetectionBar;
    private Image alertbarFill, detectedFillRed, detectedFillYellow;
    [SerializeField] private Gradient alertbarGradient;
    Transform playerTransform;
    Vector2 enemyScreenPosition;
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerDetectionBar = transform.Find("Canvas").Find("EnemyAlert").GetComponent<Slider>();
        alertbarFill = transform.Find("Canvas").Find("EnemyAlert").Find("Alert").Find("AlertFill").GetComponent<Image>();
        detectedFillRed = transform.Find("Canvas").Find("EnemyAlert").Find("Detected").Find("DetectedFillRed").GetComponent<Image>();
        detectedFillYellow = transform.Find("Canvas").Find("EnemyAlert").Find("Detected").Find("DetectedFillYellow").GetComponent<Image>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        enemyScript = gameObject.GetComponent<EnemyScript>();
        firePoint = transform.Find("FirePoint");
        
        playerDetectionBar.value = 0;
    }

    void Update()
    {
        PlayerCheck();

        alertbarFill.color = alertbarGradient.Evaluate(playerDetectionBar.value);
    }

    //Makes the enemy look to the direction of the bullet impact and increases the detection range.
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            if (col.gameObject.transform.position.x < transform.position.x && enemyScript.facingRight)
            {
                FlipEnemy();
            }
            else if (col.gameObject.transform.position.x > transform.position.x && !enemyScript.facingRight)
            {
                FlipEnemy();
            }
            if (playerDetectionBar.value <= 0.5f)
            {
                playerDetectionBar.value = 0.5f;
            }
            enemyScript.detectionDistance = 24;
        }
    }

    //Player Check.
    public void PlayerCheck()
    {
        //Checking whether the enemy is out of the screen.
        enemyScreenPosition = cam.WorldToScreenPoint (transform.position);
        if (enemyScreenPosition == null)
        {
            return;
        }

        if (enemyScreenPosition.x > Screen.width || enemyScreenPosition.x < 0)
        {
            playerDetectionBar.value = 0;
        }

        //These code snippets execute only if enemy is on screen.
        else if (enemyScript.facingRight)
        {
            //Detection bar fills up if player is in close range.
            if (Physics2D.Raycast (firePoint.position, Vector2.right, enemyScript.alertDistance, 1 << LayerMask.NameToLayer("Player")))
            {
                playerDetectionBar.value = 1;;
            }
            //Detection Barr fills up gradually when player is on enemy patrol range.
            else if (Physics2D.Raycast (firePoint.position, Vector2.right, enemyScript.detectionDistance, 1 << LayerMask.NameToLayer("Player")))
            {
                playerDetectionBar.value += enemyScript.playerDetectionSpeed;
            }
            //Detection bar recede.
            else
            {
                playerDetectionBar.value -= enemyScript.playerDetectionSpeed / 10;
            }
        }
        else
        {
            //Detection bar fills up if player is on close range.
            if (Physics2D.Raycast (firePoint.position, Vector2.left, enemyScript.alertDistance, 1 << LayerMask.NameToLayer("Player")))
            {
                playerDetectionBar.value = 1;;
            }
            //Detection bar fills up gradually when player is on enemy patrol range.
            else if (Physics2D.Raycast (firePoint.position, Vector2.left, enemyScript.detectionDistance, 1 << LayerMask.NameToLayer("Player")))
            {
                playerDetectionBar.value += enemyScript.playerDetectionSpeed;
            }
            //Detection bar recede.
            else
            {
                playerDetectionBar.value -= enemyScript.playerDetectionSpeed / 10;
            }
        }

        #region Detection Bar GFX.
        if (playerDetectionBar.value == 0)
        {
            enemyScript.shootPlayer = false;
            enemyScript.playerDetected = false;
            enemyScript.playerDetected = false;
            detectedFillRed.enabled = false;
            detectedFillYellow.enabled = false;

            //Changes the detection distance to the default value if player is no more in sight.
            enemyScript.detectionDistance = enemyScript.defaultDetectionDistance;
        }
        else if (playerDetectionBar.value > 0.0f && playerDetectionBar.value < 1)
        {
            enemyScript.playerDetected = true;
            enemyScript.shootPlayer = false;
            detectedFillYellow.enabled = true;
            detectedFillRed.enabled = false;
        }
        else if (playerDetectionBar.value == 1)
        {
            enemyScript.playerDetected = true;
            enemyScript.shootPlayer = true;
            detectedFillRed.enabled = true;
            detectedFillYellow.enabled = false;
        }
        #endregion
        
        //Changing Enemy direction according to Player position when detected.
        if (enemyScript.playerDetected)
        {
            if (enemyScript.facingRight && transform.position.x - playerTransform.position.x > 0)
            {
                FlipEnemy();
            }
            else if (!enemyScript.facingRight && transform.position.x - playerTransform.position.x < 0)
            {
                FlipEnemy();
            }
        }
    }

    //To change Enemy direction when following Player and when bullet is impacted.
    void FlipEnemy()
    {
        enemyScript.facingRight = !enemyScript.facingRight;
        transform.Rotate (0,180,0);
        enemyScript.canvas.transform.Rotate (0,180,0);
    }

}
