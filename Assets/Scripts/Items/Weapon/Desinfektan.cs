using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Desinfektan : MonoBehaviour
{
    public ParticleSystem particle;
    public GameObject DesinfektanCD;
    public float sprayTime = 5f;
    public float sprayDelay = 2f;
    public float currentDelay = 2f;
    public float currentTime = 5f;

    private Animator animator;
    private bool spraying;
    private bool canSpray;
    
    private void Awake()
    {
        canSpray = true;
        particle = GetComponent<ParticleSystem>();        
    }

    private void OnEnable()
    {
        animator = DesinfektanCD.GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canSpray)
            {
                Spray();
                spraying = true;
            }                       
        }

        else if (Input.GetMouseButtonUp(0))
        {
            spraying = false;
            sprayTime = 5f;
            Stop();                 
        }

        if (spraying)
        {
            sprayTime -= Time.deltaTime;
            if (sprayTime <= 0)
            {
                canSpray = false;
                animator.SetBool("isCD", true);
                Stop();                
            }
        }

        if (canSpray == false)
        {            
            spraying = false;
            sprayDelay -= Time.deltaTime;
            if (sprayDelay <= 0)
            {
                animator.SetBool("isCD", false);
                canSpray = true;
                sprayDelay = 2f;
                currentDelay = 2f;
            }
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

    private void Spray()
    {        
        particle.Play();
        FindObjectOfType<AudioManager>().PlaySound("Desinfektan");        
    }

    private void Stop()
    {                
        particle.Stop();
        FindObjectOfType<AudioManager>().StopSound("Desinfektan");
    }
}
