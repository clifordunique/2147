using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    float speed = 10;
    GameManagerScript gameManagerScript;
    Rigidbody2D rb;
    float damage = 8;
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();

        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManagerScript.ChangeHealth(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.layer != 13 && other.gameObject.layer != 12)
        {
            Destroy(gameObject);
        }
    }
}
