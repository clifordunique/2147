using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser1 : MonoBehaviour
{
    Rigidbody2D rb;
    EnemyScript enemyScript;
    Camera mainCamera;
    [SerializeField] float speed;
    private int sweeperDamage;
    Vector2 laserPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        sweeperDamage = 50;

        rb.velocity = transform.right * speed;
        Destroy(gameObject, 2.5f);
    }
    void Update()
    {
        laserPos = mainCamera.WorldToScreenPoint(transform.position);
        if (laserPos.x > Screen.width || laserPos.x < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemyScript = other.GetComponent<EnemyScript>();
            enemyScript.StartCoroutine("ChangeEnemyColor");
            enemyScript.ChangeHealth(sweeperDamage);
        }
    }
}
