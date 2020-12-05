using UnityEngine;

public class SkinIconScaleChange : MonoBehaviour
{
    RectTransform skinIconRectTransform;
    Vector3 scaleChange;
    float width;
    float centre;
    float posDiff;
    float changeFactor;
    float temp;
    bool tickPlayed;

    void Awake()
    {
        skinIconRectTransform = gameObject.GetComponent<RectTransform>();
    }
    void Start()
    {
        width = Screen.width;
        centre = width / 2;
    }

    void Update()
    {
        posDiff = Mathf.Abs(centre - transform.position.x);
        
        changeFactor = posDiff / (width * 0.2375f);

        if (changeFactor >= 1)
        {
            scaleChange = new Vector3 (0.6f,0.6f,0.6f);
        }
        else
        {
            temp = 1 - (0.4f * changeFactor);
            scaleChange = new Vector3 (temp, temp, 1);
        }
        
        //Playing Tick Sound when skin Icon is enlarged.
        if (temp >= 0.9f && !tickPlayed)
        {
            AudioManager.instance.Play("SkinSelect");
            tickPlayed = true;
        }
        if (temp <= 0.8f)
        {
            tickPlayed = false;
        }
        
        skinIconRectTransform.localScale = scaleChange;
    }
}
//0.2375
