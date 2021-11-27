using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesinfektanBullet : MonoBehaviour
{
    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        particle.Play();
    }

    public void Stop()
    {
        particle.Stop();
    }
}
