using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlide : MonoBehaviour
{
    RectTransform panelRectTransform;
    Vector2 speed;
    [HideInInspector] public bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        panelRectTransform = gameObject.GetComponent<RectTransform>();

        speed = new Vector2(10,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (disabled)
            {
            if (panelRectTransform.anchoredPosition.x > -600)
            {
                speed.x = (panelRectTransform.anchoredPosition.x - 600) / 30;
                panelRectTransform.anchoredPosition += speed;
            }
        }
    }

    public void OnCampaignClick()
    {
        disabled = true;
    }
}
