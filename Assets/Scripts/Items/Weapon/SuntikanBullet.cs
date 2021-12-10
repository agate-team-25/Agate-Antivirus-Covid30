using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuntikanBullet : MonoBehaviour
{
    private float speed = 14f;
    public Rigidbody2D rb;
    public float time = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        // decrease counter
        time -= Time.deltaTime;

        if (time <= 0)
        {
            // destroy object if counter reach zero
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Non Physical")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ReduceHealth(1);
            }
        }

        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
        }
    }
}
