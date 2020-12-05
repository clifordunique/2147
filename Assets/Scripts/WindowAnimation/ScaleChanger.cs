using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    RectTransform gameObjectRectransform;
    Vector3 scaleChanger;
    bool enlarging;
    int buffer = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameObjectRectransform = gameObject.GetComponent<RectTransform>();
        
        scaleChanger = new Vector3(0.0007f, 0.0007f, 0);
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enlarging)
        {
            gameObjectRectransform.localScale += scaleChanger;
            if (gameObjectRectransform.localScale.x >= 1)
            {
                enlarging = false;
            }
        }
        else
        {
            gameObjectRectransform.localScale -= scaleChanger;
            if (gameObjectRectransform.localScale.x <= 0.98f)
            {
                enlarging = true;
            }
        }
    }
}
