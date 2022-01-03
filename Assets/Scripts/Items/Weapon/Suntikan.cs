using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suntikan : MonoBehaviour
{
    public Transform shootPoint;
    public SuntikanBullet suntikanBullet;
    public bool canShoot = true;    
    public GameObject GunCD;

    private Animator animator; 
    private float delay = 1f;
    private bool shoot = false;
    private static readonly float shootDelay = 1f;

    private void OnEnable()
    {
        animator = GunCD.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        FindObjectOfType<AudioManager>().StopSound("Desinfektan");
        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                Shoot();
                canShoot = false;             
            }
        }

        if(canShoot == false)
        {                        
            if (!shoot)
            {
                animator.SetBool("isCD", true);
                shoot = true;
            }

            delay -= Time.deltaTime;

            if (delay <= 0)
            {
                animator.SetBool("isCD", false);
                canShoot = true;
                shoot = false;
                delay = 1f;
            }
        }
    }

    private void Shoot()
    {
        Instantiate(suntikanBullet, shootPoint.position, shootPoint.rotation);
        FindObjectOfType<AudioManager>().PlaySound("Gun_Shoot");
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    private IEnumerator ShootDelays()
    {
        animator.SetBool("isCD", true);
        float counter = 0f;
        float waitTime = animator.GetCurrentAnimatorStateInfo(0).length;
        while(counter < waitTime)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isCD", false);
        canShoot = true;
        shoot = false;
    }

    private IEnumerator WaitForCoolDown(System.Action onCompleted)
    {
        float time = 0.0f;

        // run animation on next frame for safety reason
        yield return new WaitForEndOfFrame();

        animator.SetBool("isCD", true);

        while (time < shootDelay)
        {            
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        animator.SetBool("isCD", false);
        canShoot = true;
        onCompleted?.Invoke();
    }
}