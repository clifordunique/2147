using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    EnemyScript enemyScript;
    ActionManager actionManager;
    public float speed = 250f;
    GameObject gameManager, target;
    [SerializeField] private GameObject explosion;
    Rigidbody2D rb;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController");

        actionManager = GameObject.FindWithTag("GameController").GetComponent<ActionManager>();
        
        rb = GetComponent<Rigidbody2D>();

        target = actionManager.enemiesOnScreen[0];
        actionManager.enemiesOnScreen.RemoveAt(0);
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            Destroy (gameObject);
        }
        Vector2 direction = (Vector2) target.transform.position - rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = rotateAmount * 200;
        rb.velocity = -transform.up * speed;

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.transform.position.x == target.transform.position.x && other.transform.position.y == target.transform.position.y)
        {
            enemyScript = other.GetComponent<EnemyScript>();
            enemyScript.ChangeHealth(30);
            
            //Instantiate (explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
