using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkinData
{
    float selectedSkin;
    bool[] unlockedSkins;

    public SkinData()
    {
        unlockedSkins = new bool[10];

        for (int i = 0; i < 10; i++)
        {
            unlockedSkins[i] = false;
        }
    }
}
