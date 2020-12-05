using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameGizmos : MonoBehaviour
{
    Vector2 leftBound, rightBound, topBound, bottomBound;
    Vector2  topLeft, topRight, bottomLeft, bottomRight;

    public void Awake()
    {
        Debug.Log("Awake Function for FrameGizmos Script");
        leftBound = gameObject.transform.Find("LeftBound").transform.position;
        rightBound = gameObject.transform.Find("RightBound").transform.position;
        topBound = gameObject.transform.Find("TopBound").transform.position;
        bottomBound = gameObject.transform.Find("BottomBound").transform.position;

        topLeft.x = bottomLeft.x = leftBound.x;
        topLeft.y = topRight.y = topBound.y;
        bottomLeft.y = bottomRight.y = bottomBound.y;
        topRight.x = bottomRight.x = rightBound.x;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color (0,1,1);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
