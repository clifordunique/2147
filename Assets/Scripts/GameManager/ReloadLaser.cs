using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadLaser : MonoBehaviour
{
    float fillAmount = 1;
    Image reloadBar, foreGroundImage;
    PlayerAction playerAction;
    PlayerScript playerScript;
    GameObject fill;
    PowerUpCount count;
    bool reloadBool;

    void Start()
    {
        playerAction = GameObject.FindWithTag("Player").GetComponent<PlayerAction>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        reloadBar = transform.Find("GFX").Find("Fill").gameObject.GetComponent<Image>();
        fill = transform.Find("GFX").Find("Fill").gameObject;
        foreGroundImage = transform.Find("GFX").Find("ForeGroundImage").gameObject.GetComponent<Image>();

        count = transform.Find("Count").GetComponent<PowerUpCount>();
        count.ChangeCount(playerScript.laserCount);
    }

    void Update()
    {

        //Function for starting Reload GFX.
        if (!playerAction.laserFlag && !reloadBool)
        {
            count.ChangeCount(playerAction.currentLaserCount);
            reloadBool = true;
            fillAmount = 0;
            InvokeRepeating ("Reload", 0, 0.2f);
        }

        //When Player is Out of Power-Ups
        if (playerAction.currentLaserCount == 0)
        {
            fill.SetActive(false);
            foreGroundImage.color = new Color (0.6f,0.6f,0.6f);
        }

        reloadBar.fillAmount = fillAmount;
    }

    //Function for reload GFX.
    void Reload()
    {
        fillAmount += 0.1f;
        if (fillAmount >= 1)
        {
            reloadBool = false;
            playerAction.laserFlag = true;
            CancelInvoke("Reload");
        }
    }
}
