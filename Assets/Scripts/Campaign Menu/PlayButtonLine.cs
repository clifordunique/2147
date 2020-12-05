using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonLine : MonoBehaviour
{
    RectTransform lineRectTransform;
    Vector3 speed;


    void Start()
    {
        lineRectTransform = gameObject.GetComponent<RectTransform>();

        lineRectTransform.anchoredPosition = new Vector3(-75, 0, 0);
        speed = new Vector3(0,0,0);
    }

    void FixedUpdate()
    {
        speed.x = lineRectTransform.anchoredPosition.x + 1.5f;

        lineRectTransform.anchoredPosition = speed;
        if (lineRectTransform.anchoredPosition.x >= 70)
        {
            lineRectTransform.anchoredPosition = new Vector3(-80, 0, 0);
        }

    }
}