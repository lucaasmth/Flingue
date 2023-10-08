using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyFieldOfView _fov;
    private Rigidbody _rb;
    private EnemyShooting _shooting;
    private EnemyVisibility _visibility;
    private bool _canShoot;

    void Start()
    {
        _fov = GetComponent<EnemyFieldOfView>();
        if (_fov == null)
        {
            Debug.LogError("Enemy is blind! (No FOV)");
        }
        _rb = GetComponent<Rigidbody>();
        _shooting = GetComponent<EnemyShooting>();
        if (_shooting != null) _canShoot = true;
        _visibility = GetComponent<EnemyVisibility>();
    }

    void Update()
    {
        Vector3 lastPlayerPosition = _fov.LastPlayerPosition;
        if (lastPlayerPosition.Equals(Vector3.zero)) return;
        
        // Turn to player
        transform.LookAt(lastPlayerPosition);
        
        // Move to player
        if (!_fov.CanSeePlayer)
        {
            MoveTo(lastPlayerPosition, .5f);
        }
        else
        {
            MoveTo(lastPlayerPosition, 5f);
            // Shoot at player
            if (_canShoot)
            {
                StartCoroutine(RefreshWeapon());
            }
        }
    }

    private void MoveTo(Vector3 position, float minDist)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, position);
        if (distanceToPlayer < minDist) return;
        
        // Move towards player
        Vector3 directionToPlayer = (position - transform.position).normalized;
        _rb.MovePosition(_rb.position + directionToPlayer * (5f * Time.deltaTime));
    }

    private IEnumerator RefreshWeapon()
    {
        _canShoot = false;
        yield return new WaitForSeconds(.7f);
        _shooting.Shoot();
        _visibility.SetVisible(2f);
        yield return new WaitForSeconds(.3f);
        _canShoot = true;
    }
}
