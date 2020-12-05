using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceData
{
    public int electricTotalCount;
    public int nuclearTotalCount;
    public ResourceData (ResourceData resourceData)
    {
        electricTotalCount = resourceData.electricTotalCount;
        nuclearTotalCount = resourceData.nuclearTotalCount;
    }

    public ResourceData()
    {
        electricTotalCount = 0;
        nuclearTotalCount = 0;
    }
}
