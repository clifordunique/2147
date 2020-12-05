using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser2 : MonoBehaviour
{
    EnemyScript enemyScript;
    GameObject player;
    private int laserDamage = 15;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameObject.transform.parent = player.transform;

        Destroy(gameObject, 0.75f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Enemy Hit By Laser.");
            enemyScript.StartCoroutine("ChangeEnemyColor");
            enemyScript = other.GetComponent<EnemyScript>();
            enemyScript.ChangeHealth(laserDamage);
        }
    }

}
