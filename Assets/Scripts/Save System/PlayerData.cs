using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    float maxHealth;
    float lifeCount;
    float pistolDamage;
    float laserDamage;
    float sweeperDamage;
    float shieldDuration;
    float missileDamage;

    public PlayerData()
    {
        maxHealth = 200f;
        lifeCount = 2;
        pistolDamage = 20f;
        laserDamage = 40f;
        sweeperDamage = 30f;
        shieldDuration = 10f;
        missileDamage = 50f;
    }
}
