using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadShield : MonoBehaviour
{
    float fillAmount = 1;
    private Image reloadBar, foreGroundImage;
    PlayerAction playerAction;
    PlayerScript playerScript;
    ActionManager actionManager;
    PowerUpCount count;
    bool reloadBool;
    GameObject fill;
    void Start()
    {
        actionManager = GameObject.FindWithTag("GameController").GetComponent<ActionManager>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        playerAction = GameObject.FindWithTag("Player").GetComponent<PlayerAction>();

        reloadBar = transform.Find("GFX").Find("Fill").gameObject.GetComponent<Image>();
        fill = transform.Find("GFX").Find("Fill").gameObject;
        foreGroundImage = transform.Find("GFX").Find("ForeGroundImage").gameObject.GetComponent<Image>();

        count = transform.Find("Count").GetComponent<PowerUpCount>();
        count.ChangeCount(playerScript.shieldCount);
    }

    void Update()
    {

        //Function for starting Reload GFX;
        //ShieldFlag is false when shield is activated.
        if (!actionManager.shieldFlag && !reloadBool)
        {
            count.ChangeCount(playerAction.currentShieldCount);
            reloadBool = true;
            fillAmount = 0;
            InvokeRepeating ("Reload", 0, 0.05f);
        }
        
        if (playerAction.currentShieldCount == 0)
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
            actionManager.shieldFlag = true;
            CancelInvoke("Reload");
        }
    }
}
