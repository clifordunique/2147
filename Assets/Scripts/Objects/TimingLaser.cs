using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingLaser : MonoBehaviour
{
    //To Change the Player health.
    GameManagerScript gameManagerScript;
    GameObject laser;
    GameObject laserGlow;
    SpriteRenderer playerSprite;
    Color hurtColor;
    Vector2 top;
    RaycastHit2D hitInfo, hit2D;
    //Boolean for Dealing damage only once when laser is conitnually hitting.
    //Damage is Dealed only when bool is true.
    bool damageBool = true;
    float laserDamage;
    float dif;
    float flipInterval = 2.5f;
    public float startInterval;
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
        laser = transform.Find("Top").Find("Laser").gameObject;
        laserGlow = transform.Find("Top").Find("LaserGlow").gameObject;
        playerSprite = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        
        top = transform.Find("Top").position;

        SetEdges();
        hurtColor = new Color(1, 0.5f, 0.5f);
        laserDamage = 5;

        InvokeRepeating("FlipState", startInterval, flipInterval);
    }

    void Update()
    {
        if (laser.activeSelf)
        {
            hit2D = Physics2D.Linecast(top, hitInfo.point, 1 << LayerMask.NameToLayer("Player"));
            if (hit2D)
            {
                //Player Turns Red when hitting the Laser.
                playerSprite.color = hurtColor;
                if (damageBool)
                {
                    //Handheld.Vibrate();
                    gameManagerScript.ChangeHealth(laserDamage);
                    
                    //Bool set to false after changing the health once.
                    damageBool = false;
                }
            }
            //damageBool is false only when the player has come into collision with the laser obstacle.
            else if (!damageBool)
            {
                //Player color needs to be changed to normal only if it is escaping from the laser obstacle.
                //Otherwise no need. Other collision with Obstacles may need to change to hurtcolor.
                playerSprite.color = new Color (1,1,1);

                //Bool is set to true when player leaves from the impact of the laser.
                //So that damage is dealed when player is contact again without the need of laser disabling and re-enabling..
                damageBool = true;
            }
        }
        //damageBool is false only when the player has come into collision with the laser obstacle.
        else if (!damageBool)
        {
            //Player color needs to be changed to normal only if it is escaping from the laser obstacle.
            //Otherwise no need. Other collision with Obstacles may need to change to hurtcolor.
            playerSprite.color = new Color (1,1,1);
            damageBool = true;
        }
    }

    void SetEdges()
    {
        hitInfo = Physics2D.Raycast (top, Vector2.down, 30, 1 << LayerMask.NameToLayer("Ground"));
        
        if (hitInfo)
        {
            dif = top.y - hitInfo.point.y;
            Vector3 scaleChange = new Vector3 (0, -dif * 0.84f, 0);
            laser.transform.localScale += scaleChange;
            laserGlow.transform.localScale += scaleChange;
        }
    }

    void FlipState()
    {
        if (laser.activeSelf)
        {
            laser.SetActive(false);
            laserGlow.SetActive(false);
        }
        else
        {
            laser.SetActive(true);
            laserGlow.SetActive(true);
        }
    }
}
