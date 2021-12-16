using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suntikan : MonoBehaviour
{
    public Transform shootPoint;
    public SuntikanBullet suntikanBullet;
    public bool canShoot = true;
    private float delay = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                Shoot();
                canShoot = false;
                StartCoroutine(ShootDelay());
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
}