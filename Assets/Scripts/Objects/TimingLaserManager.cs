using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script to manage the group of Lasers.
public class TimingLaserManager : MonoBehaviour
{
    float laserDamage;
    //To change the Player Health.
    GameManagerScript gameManagerScript;
    //Boolean for Dealing damage only once when laser is conitnually hitting.
    //Damage is Dealed only when bool is true.
    [HideInInspector] public bool damageBool = true;
    float damage = 10;
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
    }

    void Update()
    {
        
    }
    public void DealDamage()
    {
        gameManagerScript.ChangeHealth(damage);
    }
}
