using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaChange : MonoBehaviour
{
    Image titleLine;
    bool blink;
    Color color;
    void Start()
    {
        titleLine = gameObject.GetComponent<Image>();
        
        color = titleLine.color;
    }

    void FixedUpdate()
    {
        if (blink)
        {
            color.a -= 0.02f;
            titleLine.color = color;
            
            if (color.a <= 0.5f)
            {
                blink = false;
            }
        }
        else
        {
            color.a += 0.02f;
            titleLine.color = color;
            
            if (color.a >= 1f)
            {
                blink = true;
            }
        }
    }
}
