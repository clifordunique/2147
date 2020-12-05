using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    //To Change the Player health.
    GameManagerScript gameManagerScript;
    GameObject laser;
    GameObject laserGlow;
    SpriteRenderer playerSprite;
    Color hurtColor;
    Vector2 top, bottom;
    //Boolean for Dealing damage only once when laser is conitnually hitting.
    //Damage is Dealed only when bool is true.
    bool damageBool = true;
    float laserDamage;
    float dif;
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
        laser = transform.Find("Bottom").Find("Laser").gameObject;
        laserGlow = transform.Find("Bottom").Find("LaserGlow").gameObject;
        playerSprite = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        
        top = transform.Find("Top").position;
        bottom = transform.Find("Bottom").position;

        SetEdges();
        hurtColor = new Color(1, 0.5f, 0.5f);
        laserDamage = 5;

        InvokeRepeating("FlipState", 0, 3);
    }

    void Update()
    {
        if (laser.activeSelf)
        {
            RaycastHit2D hit2D = Physics2D.Linecast(top, bottom, 1 << LayerMask.NameToLayer("Player"));
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
        dif = (top.y - bottom.y);
        Vector3 scaleChange = new Vector3 (0, dif * 0.84f, 0);
        laser.transform.localScale += scaleChange;
        laserGlow.transform.localScale += scaleChange;
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
