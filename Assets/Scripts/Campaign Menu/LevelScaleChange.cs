using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScaleChange : MonoBehaviour
{
    RectTransform panelRectTransform;
    Vector3 scaleChange;
    float width;
    float centre;
    float posDiff;
    float changeFactor;
    float temp;

    void Awake()
    {
        panelRectTransform = gameObject.GetComponent<RectTransform>();
    }
    void Start()
    {
        width = Screen.width;
        centre = width / 2;
    }

    void Update()
    {
        posDiff = Mathf.Abs(centre - transform.position.x);

        changeFactor = posDiff / (centre);

        if (changeFactor >= 1)
        {
            scaleChange = new Vector3 (0.7f,0.7f,0.7f);
        }
        else
        {
            temp = 1 - (0.3f * changeFactor);
            scaleChange = new Vector3 (temp, temp, 1);
        }
        
        panelRectTransform.localScale = scaleChange;
    }
}