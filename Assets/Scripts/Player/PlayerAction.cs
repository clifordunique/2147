using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    Camera mainCamera;
    float  yOffset;
    PlayerScript playerScript;
    ReloadFire reloadFire;
    ActionManager actionManager;
    Transform firePoint;
    [SerializeField] private GameObject muzzleFlash;
    #region Weapon List
    [Header("Weapon 1")]
    [SerializeField] GameObject bulletRed;
    [Header("Weapon 2")]
    [SerializeField] GameObject laser1;  
    Transform laserBlue, laserGlow;
    GameObject laserBlueGameObject, laserGlowGameObject;
    #endregion
    // Boolean variable to track the reloading of the weapon.
    [HideInInspector] public bool redFlag = true, sweeperFlag = true, laserFlag = true;
    [HideInInspector] public int currentMag, currentSweepCount, currentLaserCount, currentShieldCount, currentMissileCount, currentSlowMoCount;
    private float deltaFireTime = 0.075f;
    private int laserDamage = 15;
    private bool canFire = true;
    Animator animator;
    RaycastHit2D hitInfo;
    private Vector3 scaleChange;
    private Vector3 firePos;
    float sweeperPoint1;
    float sweeperPoint2;
    float sweeperSize;
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        firePoint = gameObject.transform.Find("FirePoint").GetComponent<Transform>();
        laserBlue = transform.Find("Laser_Blue").GetComponent<Transform>();
        laserBlueGameObject = transform.Find("Laser_Blue").gameObject;
        laserGlow = transform.Find("Laser_Blue_Glow").GetComponent<Transform>();
        laserGlowGameObject = transform.Find("Laser_Blue_Glow").gameObject;
        DisableLaser();

        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        actionManager = GameObject.FindWithTag("GameController").GetComponent<ActionManager>();
        reloadFire = GameObject.FindWithTag("Canvas").transform.Find("Power-Ups").Find("Bullet").GetComponent<ReloadFire>();

        animator = GetComponent<Animator>();

        currentMag = playerScript.redMag;
        currentSweepCount = playerScript.sweepCount;
        currentLaserCount = playerScript.laserCount;
        currentShieldCount = playerScript.shieldCount;
        currentMissileCount = playerScript.missileCount;
        currentSlowMoCount = playerScript.slowMoCount;
    }
    void Update()
    {
        if (playerScript.firePressed  && canFire)
        {
            //Setting a bool so that bullets are fired at an interval.
            canFire = false;
            Shoot();
            Invoke("ChangeCanFire", deltaFireTime);
        }
    }

    void ChangeCanFire()
    {
        canFire = true;
    }


    //Function to Shoot According to Fireselect in GameManager
    void Shoot()
    {
        //Code for firing Bullet.
        if (actionManager.red && redFlag)
        {
            //Fire Animation is played only if the player is idle.
            //otherwise muzzleflash is instantiated instead of playing the animation.
            if (Mathf.Abs(playerScript.horizontal) > 0 || playerScript.isCrouching)
            {
                //newPos is the firepoint position when running or crouching, i.e, y value is 0.15 less
                //newPos is used to instantiate muzzlFlash
                //randFirePosition is bullet spawn position by randomizing the y value of newPos Vector
                if (Mathf.Abs(playerScript.horizontal) > 0)
                {
                    yOffset = 0.15f;
                }
                else
                {
                    //yOffset = 0f;
                    yOffset = 0.8f;
                }
                Vector3 newPos;
                Vector3 randFirePosition;
                newPos.x = firePoint.position.x;
                newPos.y = firePoint.position.y - yOffset;
                newPos.z = firePoint.position.z;
                var newMuzzleFlash = Instantiate(muzzleFlash, newPos, firePoint.rotation);
                newMuzzleFlash.transform.SetParent(gameObject.transform);
                randFirePosition.x = firePoint.position.x;
                randFirePosition.y = firePoint.position.y - yOffset + Random.Range(-0.2f,0.2f);
                randFirePosition.z = firePoint.position.z;
                Instantiate(bulletRed, randFirePosition, firePoint.rotation);
            }
            //When idle.
            else
            {
                Vector3 randFirePosition;
                animator.SetTrigger ("Fire");
                randFirePosition.x = firePoint.position.x;
                randFirePosition.y = firePoint.position.y + Random.Range(-0.2f,0.2f);
                randFirePosition.z = firePoint.position.z;
                Instantiate(bulletRed, randFirePosition, firePoint.rotation);
                
            }
            currentMag--;
            reloadFire.ShootUIGFX();
            if (currentMag == 0)
            {
                redFlag = false;
                currentMag = playerScript.redMag;
            }
        }
        
        //Code for firing sweeper.
        if (actionManager.laser1 && sweeperFlag)
        {
            if (currentSweepCount > 0)
            {
                hitInfo = Physics2D.Raycast (firePoint.position, Vector2.down, 20, 1 << LayerMask.NameToLayer("Ground"));
                sweeperPoint2 = hitInfo.point.y;
                hitInfo = Physics2D.Raycast (firePoint.position, Vector2.up, 20, 1 << LayerMask.NameToLayer("Ground"));
                if (hitInfo)
                {
                    sweeperPoint1 = hitInfo.point.y;
                }
                else
                {
                    sweeperPoint1 = mainCamera.ScreenToWorldPoint(new Vector3 (0, Screen.height, 0)).y;
                }
                sweeperSize = sweeperPoint1 - sweeperPoint2;
                scaleChange = new Vector3 (0, sweeperSize * 0.027f, 0);
                firePos.x = firePoint.position.x;
                firePos.y = sweeperPoint1;

                currentSweepCount--;
                
                var laserInScene = Instantiate(laser1, firePos, firePoint.rotation);
                laserInScene.transform.localScale += scaleChange;
                animator.SetTrigger ("Fire");
                sweeperFlag = false;
                
                actionManager.Red();
            }
        }
        
        //Code for firing laser.
        if (actionManager.laser2 && laserFlag)
        {
            if (currentLaserCount > 0)
            {
                laserBlueGameObject.SetActive(true);
                laserGlowGameObject.SetActive(true);

                currentLaserCount--;
                hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, 15, 1 << LayerMask.NameToLayer("Enemy"));
                if (hitInfo)
                {
                    scaleChange = new Vector3 (0, hitInfo.distance, 0);
                    
                    laserBlue.localScale += scaleChange;
                    laserGlow.localScale += scaleChange;

                    hitInfo.transform.GetComponent<EnemyScript>().ChangeHealth(laserDamage);
                    hitInfo.transform.GetComponent<EnemyScript>().StartCoroutine("ChangeEnemyColor");
                }
                else
                {
                    scaleChange = new Vector3 (0, 15, 0);
                    
                    laserBlue.localScale += scaleChange;
                    laserGlow.localScale += scaleChange;
                }
                animator.SetTrigger ("Fire");
                Invoke("DisableLaser", 0.1f);
                laserFlag = false;

                actionManager.Red();
            }
        }
    }

    void DisableLaser()
    {
        laserBlue.localScale = new Vector3 (3,0,0);
        laserGlow.localScale = new Vector3 (3,0,0);
        
        laserBlueGameObject.SetActive(false);
        laserGlowGameObject.SetActive(false);
    }
}
