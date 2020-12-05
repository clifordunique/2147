using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] GameObject enemyBullet;
    EnemyScript enemyScript;
    Transform firePoint;

    bool reloaded = true;
    private int currentMag;

    void Start()
    {
        enemyScript = gameObject.GetComponent<EnemyScript>();
        firePoint = transform.Find("FirePoint");
    }

    void Update()
    {
        if (enemyScript.shootPlayer)
        {
            if (reloaded)
            {
                reloaded = false;
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.25f);

        currentMag ++;
        if (enemyScript.isDead)
        {
            yield break;
        }
        var bullet = Instantiate (enemyBullet, firePoint.position, firePoint.rotation);
        bullet.GetComponent<EnemyBullet>().damage = enemyScript.bulletDamage;

        if(currentMag == enemyScript.magCapacity)
        {
            currentMag = 0;
            yield return new WaitForSeconds(1);
            reloaded = true;
        }
        else
        {
            reloaded = true;
        }
    }
}
