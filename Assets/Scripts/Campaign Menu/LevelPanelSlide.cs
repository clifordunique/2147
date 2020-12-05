using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelPanelSlide : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Transform levelPanelTransform;
    Vector3 levelPanelLocation;
    Vector3 dragDifference;
    Vector3 newPos;
    float difference;
    [SerializeField] float threshold;
    [SerializeField] float smoothing;
    float percentile;
    int totalLevel;
    int curLevel;
    // Start is called before the first frame update
    void Start()
    {
        curLevel = 1;
        totalLevel = 4;

        levelPanelTransform = transform.Find("Level Drag Panel").transform;
        //levelPanelLocation = levelPanelTransform.position;
        newPos = levelPanelTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData data)
    {
        difference = data.pressPosition.x - data.position.x;
        dragDifference.x = difference;
        //levelPanelTransform.position = levelPanelLocation - dragDifference;
        levelPanelTransform.position = new Vector3 (newPos.x - difference, levelPanelTransform.position.y, levelPanelTransform.position.z);
    }

    //public void OnEndDrag(PointerEventData data)
    //{
    //    levelPanelLocation = levelPanelTransform.position;
    //}

    public void OnEndDrag(PointerEventData data)
    {
        percentile = (data.pressPosition.x - data.position.x) / (Screen.width);
        if (Mathf.Abs(percentile) >= threshold)
        {
            if (percentile > 0 && curLevel < totalLevel)
            {
                newPos += new Vector3 (-Screen.width * 0.625f, 0, 0);   
                curLevel++;
            }
            else if (percentile < 0 && curLevel > 1)
            {
                newPos += new Vector3 (Screen.width * 0.625f, 0, 0);
                curLevel--;
            }
        }



        StartCoroutine(Smooth(levelPanelTransform.position, newPos, smoothing));
    }

    IEnumerator Smooth(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            levelPanelTransform.position = Vector3.Lerp (startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
