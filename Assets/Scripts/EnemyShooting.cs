using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject muzzleFlashPrefab;
    public float bulletSpeed = 50f;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void Shoot()
    {
        _audioSource.Play();
        GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
        Destroy(flash, .05f);
        Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, Random.Range(-5f, 5f), 0);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        bullet.GetComponent<Bullet>().SetOwner(gameObject);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
    }
}
