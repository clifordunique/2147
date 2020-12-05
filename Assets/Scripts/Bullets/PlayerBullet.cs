using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    EnemyScript enemyScript;
    [SerializeField] GameObject explosion;
    float speed;
    private int bulletDamage;
    void Start()
    {
        animator = gameObject.transform.Find("GFX").GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        bulletDamage = 20;
        speed = 15;
        
        rb.velocity = transform.right * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {

            Destroy(gameObject);
            enemyScript = other.GetComponent<EnemyScript>();
            enemyScript.ChangeHealth(bulletDamage);
            enemyScript.StartCoroutine("ChangeEnemyColor");

            //animator.Play("Bullet_Explosion");

            //rb.velocity = new Vector2(0,0);
            //Instantiate (explosion, transform.position, transform.rotation);
        }
        else
        {
            rb.velocity = new Vector2 (0,0);

            animator.Play("Bullet_Wall_Impact");
        }

    }

    public void Destroy()
    {
        Destroy (gameObject);
    }
}
