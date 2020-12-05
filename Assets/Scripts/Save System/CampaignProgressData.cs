using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CampaignProgressData
{
    float currentLevel;
    float totalBadges;
    bool[,] badgesCollected;

    public CampaignProgressData()
    {
        currentLevel = 1;
        totalBadges = 0;
        badgesCollected = new bool[7,3];
        
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                badgesCollected[i,j] = false;
            }
        }
    }
}
