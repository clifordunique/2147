using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script attached to guns in AirGun and StationaryGun
public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float fireInterval;
    [Range(1,3)]
    [SerializeField] int fireRate;
    int fireCount = 0;

    void OnEnable()
    {
        StartCoroutine (FireBullet());
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            for (fireCount = 1; fireCount <= fireRate; fireCount++)
            {
                Instantiate (bullet, transform.position, transform.rotation);

                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
