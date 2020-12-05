using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private bool isVertical;
    [Header("Movement")]
    [Range(1,3)]
    [SerializeField] private float moveSpeed;
    private float constSpeed;
    void Start()
    {
        startPos = transform.Find("Start Position").position;
        endPos = transform.Find("End Position").position;
        
        CheckDirection();

        if (isVertical)
        {
            constSpeed = 2f / (startPos.y - endPos.y);
        }
        else
        {
            constSpeed = 2f / (endPos.x - startPos.x);
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(startPos,endPos, Mathf.PingPong(Time.time * constSpeed * moveSpeed, 1));
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
