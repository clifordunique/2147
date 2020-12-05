using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject barrelExplosion;
    StressReceiver cameraStressReciever;
    public float radius;
    private int barrelDamage;
    Collider2D[] enemyColliders;
    Collider2D[] barrelColliders;
    Collider2D thisCollider;
    Barrel barrelScript;
    EnemyScript enemyScript;

    void Start()
    {
        cameraStressReciever = GameObject.Find("Player Camera").GetComponent<StressReceiver>();

        thisCollider = GetComponent<Collider2D>();

        barrelDamage = 30;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag ("Bullet"))
        {
            Explode();   
        }
    }

    public void Explode()
    {
        thisCollider.enabled = false;

        enemyColliders =  Physics2D.OverlapCircleAll (transform.position, radius, 1 << LayerMask.NameToLayer("Enemy"));

        barrelColliders = Physics2D.OverlapCircleAll (transform.position, radius/2, 1 << LayerMask.NameToLayer("Barrel"));

        foreach (Collider2D barrel in barrelColliders)
        {
            barrelScript = barrel.GetComponent<Barrel>();

            if (barrelScript != null)
            {
                barrelScript.Explode();
            }
        }

        foreach (Collider2D enemy in enemyColliders)
        {
            enemyScript = enemy.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.ChangeHealth(barrelDamage);
            }
        }

        Destroy (gameObject);
        cameraStressReciever.InduceStress (0.65f);
        Instantiate (barrelExplosion, transform.position, transform.rotation);
    }
}
