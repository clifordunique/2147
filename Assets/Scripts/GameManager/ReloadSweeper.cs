using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadSweeper : MonoBehaviour
{
    float fillAmount = 1;
    private Image reloadBar, foreGroundImage;
    PlayerAction playerAction;
    PlayerScript playerScript;
    PowerUpCount count;
    bool reloadBool;
    GameObject fill;
    void Start()
    {
        playerAction = GameObject.FindWithTag("Player").GetComponent<PlayerAction>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        reloadBar = transform.Find("GFX").Find("Fill").gameObject.GetComponent<Image>();
        fill = transform.Find("GFX").Find("Fill").gameObject;
        foreGroundImage = transform.Find("GFX").Find("ForeGroundImage").gameObject.GetComponent<Image>();

        count = transform.Find("Count").GetComponent<PowerUpCount>();
        count.ChangeCount(playerScript.sweepCount);
    }

    void Update()
    {
        //Function for starting Reload GFX;
        //Sweeperflag is false when sweeper is fired.
        if (!playerAction.sweeperFlag && !reloadBool)
        {
            count.ChangeCount(playerAction.currentSweepCount);
            reloadBool = true;
            fillAmount = 0;
            InvokeRepeating ("Reload", 0, 0.05f);
        }
        
        if (playerAction.currentSweepCount == 0)
        {
            fill.SetActive(false);
            foreGroundImage.color = new Color (0.6f,0.6f,0.6f);
        }
        
        reloadBar.fillAmount = fillAmount;
    }

    void Reload()
    {
        fillAmount += 0.01f;
        if (fillAmount >= 1)
        {
            reloadBool = false;
            playerAction.sweeperFlag = true;
            CancelInvoke("Reload");
        }
    }
}
