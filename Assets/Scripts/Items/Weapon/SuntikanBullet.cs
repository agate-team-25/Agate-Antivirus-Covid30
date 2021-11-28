using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuntikanBullet : MonoBehaviour
{
    private float speed = 10f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ReduceHealth(1);
            }
        }
    }
}
