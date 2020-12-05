using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkinPanelSlide : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Transform skinPanelTransform;
    Vector3 dragDifference;
    Vector3 newPos;
    Vector3 startPos;
    float difference;
    [SerializeField] float threshold;
    [SerializeField] float smoothing;
    float percentile;
    float numberOfSkinsDragged;
    float totalNumberOfSkin;
    float currentSkin;
    // Start is called before the first frame update
    void Start()
    {
        currentSkin = 1;
        totalNumberOfSkin = 6;

        skinPanelTransform = transform.Find("Skin Drag Panel").transform;
        startPos = skinPanelTransform.position;
        newPos = skinPanelTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData data)
    {
        difference = data.pressPosition.x - data.position.x;
        dragDifference.x = difference;

        if (difference > 0 && currentSkin < totalNumberOfSkin || difference < 0 && currentSkin > 1)
        {
            skinPanelTransform.position = new Vector3 (newPos.x - difference, skinPanelTransform.position.y, skinPanelTransform.position.z);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        percentile = (data.pressPosition.x - data.position.x) / (Screen.width);

        numberOfSkinsDragged =  (data.pressPosition.x - data.position.x) / (Screen.width * 0.2375f);
        numberOfSkinsDragged = (Mathf.Abs(Mathf.RoundToInt(numberOfSkinsDragged)));
    
        if (Mathf.Abs(percentile) >= threshold)
        {
            //When Swiping Right.
            if (percentile > 0 && currentSkin < totalNumberOfSkin)
            {
                if (numberOfSkinsDragged > totalNumberOfSkin - currentSkin)
                {
                    numberOfSkinsDragged = totalNumberOfSkin - currentSkin;
                }
                newPos += new Vector3 (-Screen.width * 0.2375f * numberOfSkinsDragged, 0, 0);
                currentSkin += numberOfSkinsDragged;
            }
            //When Swiping Left.
            else if (percentile < 0 && currentSkin > 1)
            {
                if (numberOfSkinsDragged >= currentSkin)
                {
                    numberOfSkinsDragged = currentSkin - 1;
                }
                newPos += new Vector3 (Screen.width * 0.2375f * numberOfSkinsDragged, 0, 0);
                currentSkin -= numberOfSkinsDragged;
            }
        }

        StartCoroutine(Smooth(skinPanelTransform.position, newPos, smoothing));
    }

    IEnumerator Smooth(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            skinPanelTransform.position = Vector3.Lerp (startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    public void SelectSkin(int skinNumber)
    {
        newPos = startPos + new Vector3(-Screen.width * 0.2375f * (skinNumber - 1), 0, 0);
        currentSkin = skinNumber;

        StartCoroutine(Smooth(skinPanelTransform.position, newPos, smoothing));
    }
}
