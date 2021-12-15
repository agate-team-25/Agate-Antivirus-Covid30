using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desinfektan : MonoBehaviour
{
    public ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            particle.Play();
            FindObjectOfType<AudioManager>().PlaySound("Desinfektan");
        }

        else if (Input.GetMouseButtonUp(0))
        {
            particle.Stop();
            FindObjectOfType<AudioManager>().StopSound("Desinfektan");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("Collide with something");
        if (other.tag == "Enemy")
        {
            //Debug.Log("Collide on Enemy");
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ReduceHealth(1);
            }
        }

        if (other.tag == "Projectile")
        {
            //Debug.Log("Collide on Projectile");
            Destroy(other);
        }

        if (other.tag == "Obstacle")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.GetDamage(1);
            }
        }
    }
}
