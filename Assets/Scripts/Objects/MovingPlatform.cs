using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private bool isVertical;
    [Header("Movement")]
    [Range(1f,2.5f)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitingTime;
    private float constSpeed;
    private bool lerp = true;
    private bool startWait = true, endWait = true;
    private float incrementValue;
    private float top, bottom, left, right;

    void Start()
    {
        startPos = transform.Find("Start Position").position;
        endPos = transform.Find("End Position").position;

        CheckDirection();

        if (isVertical)
        {
            constSpeed = 5 / (startPos.y - endPos.y);

            if (startPos.y >= endPos.y)
            {
                top = startPos.y;
                bottom = endPos.y;
            }
            else
            {
                top = endPos.y;
                bottom = startPos.y;
            }
        }
        else
        {
            constSpeed = 5 / (endPos.x - startPos.x);

            if (startPos.x <= endPos.x)
            {
                left = startPos.x;
                right = endPos.x;
            }
            else
            {
                left = endPos.x;
                right = startPos.x;
            }
        }
    }
    void Update()
    {
        if (lerp)
        {
            transform.position = Vector2.Lerp(startPos,endPos, Mathf.PingPong(incrementValue * constSpeed * moveSpeed, 1));
            incrementValue += 0.02f;
        }

        #region Vertical MovingPlatform
        if (isVertical && startWait && transform.position.y >= top - 0.2f)
        {
            startWait = false;
            endWait = true;
            lerp = false;
            Invoke("ChangeLerp",waitingTime);
        }
        else if (isVertical && endWait && transform.position.y <= bottom + 0.2f)
        {
            endWait = false;
            startWait = true;
            lerp = false;
            Invoke("ChangeLerp",waitingTime);
        }
        #endregion

        #region Horizontal MovingPlatform
        if (!isVertical && startWait && transform.position.x <= left + 0.2f)
        {
            startWait = false;
            endWait = true;
            lerp = false;
            Invoke("ChangeLerp",waitingTime);
        }
        else if (!isVertical && endWait && transform.position.x >= right - 0.2f)
        {
            endWait = false;
            startWait = true;
            lerp = false;
            Invoke("ChangeLerp",waitingTime);
        }
        #endregion
    }
    void ChangeLerp()
    {
        lerp = true;
    }

    void CheckDirection()
    {
        if (startPos.x == endPos.x)
        {
            isVertical = true;
        }
        else
        {
            isVertical = false;
        }
    }
}
