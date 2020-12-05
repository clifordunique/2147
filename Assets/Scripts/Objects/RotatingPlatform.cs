using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float journeyTime = 1.0f;
    public float waitingTime;
    [Range(1, 6)]
    public float radius;
    private float startTime;
    public float range;
    [SerializeField] private int totalCycles;
    private int currentCycle = 0;
    private bool lerp, lerpForward;

    Vector3 startPos, endpos;
    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
        endpos = transform.position + new Vector3(range, 0, 0);

        lerp = true;
        lerpForward = true;        
    }

    void Update()
    {
        if(lerp)
            Lerp();
    }

    void Lerp()
    {
        // The center of the arc
        Vector3 center = (startPos + endpos) * 0.5F;

        // move the center a bit downwards to make the arc vertical
        center += new Vector3(0, radius, 0);

        // Interpolate over the arc relative to center
        Vector3 riseRelCenter = startPos - center;
        Vector3 setRelCenter = endpos - center;

        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        float fracComplete = (Time.time - startTime) / journeyTime;

        if (fracComplete >= 1)
        {
            lerp =  false;

            Invoke("NextPoint", waitingTime);
        }

        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        transform.position += center;
    }

    void NextPoint()
    {

        //Incrementing or decrementing the value of current cycle depending on the direction of lerp.
        if (lerpForward)
        {
            currentCycle++;
        }
        else
        {
            currentCycle--;
        }

        //Changing the direction of lerp when cycle reaches beginning or end.
        if (currentCycle >= totalCycles)
        {
            lerpForward = false;
        }
        if (currentCycle == 0)
        {
            lerpForward = true;
        }

        startPos = transform.position;

        //Setting the end position of the cycle depending on the direction of the lerp.
        if (lerpForward)
        {
            endpos = transform.position + new Vector3(range, 0, 0);
        }
        else
        {
            endpos = transform.position - new Vector3(range, 0, 0);        
        }
        
        startTime = Time.time;
        lerp = true;
    }
}
