using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadFire : MonoBehaviour
{
    float fillAmount = 1;
    [SerializeField] Image image;
    PlayerAction playerAction;
    PlayerScript playerScript;
    bool reloadBool;

    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        playerAction = GameObject.FindWithTag("Player").GetComponent<PlayerAction>();
    }

    void Update()
    {
        //Redflag is false when current mag == 0.
        if (!playerAction.redFlag && !reloadBool)
        {
            reloadBool = true;
            fillAmount = 0;
            InvokeRepeating ("Reload", 0, 0.005f);
        }
        image.fillAmount = fillAmount;
    }

    public void ShootUIGFX()
    {
        fillAmount -= 1f/playerScript.redMag;
    }

    void Reload()
    {
        fillAmount += 0.01f;
        if (fillAmount >= 1)
        {
            reloadBool = false;
            playerAction.redFlag = true;
            CancelInvoke("Reload");
        }
    }
}
