using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desinfektan : MonoBehaviour
{
    public ParticleSystem particle;
    private PolygonCollider2D polygonCollider2D;

    private void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            particle.Play();            
            polygonCollider2D.enabled = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            particle.Stop();
            polygonCollider2D.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //something
        print("Hit object");
    }
}
