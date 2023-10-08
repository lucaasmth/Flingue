using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject muzzleFlash;
    public float bulletSpeed = 50f;
    public float cooldown = .1f;

    private bool _canShoot = true;

    private Health _health;

    private void Start()
    {
        _health = GetComponent<Health>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_health.IsDead && _canShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _canShoot = false;
        GameObject flash = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation, firePoint);
        Destroy(flash, .05f);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().SetOwner(gameObject);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(RefreshCooldown());
    }

    private IEnumerator RefreshCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        _canShoot = true;
    }
}
