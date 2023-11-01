using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private GameObject _playerRef;
    
    public bool CanSeePlayer { get; private set; }

    public Vector3 LastPlayerPosition { get; private set; }

    private void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        const float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        var position = transform.position;
        // We check if the player is in range
        Collider[] rangeChecks = Physics.OverlapSphere(position, radius, targetMask);
        if (rangeChecks.Length == 0)
        {
            CanSeePlayer = false;
            return;
        }
        
        Transform target = rangeChecks[0].transform;
        Vector3 directionToTarget = (target.position - position).normalized;
        // We check if the player is in the view angle
        if (!(Vector3.Angle(transform.forward, directionToTarget) < angle / 2))
        {
            CanSeePlayer = false;
            return;
        }
        
        float distanceToTarget = Vector3.Distance(position, target.position);
        // We check is something is hiding the player (wall)
        if (Physics.Raycast(position, directionToTarget, distanceToTarget, obstructionMask))
        {
            CanSeePlayer = false;
            return;
        }
        
        CanSeePlayer = true;
        LastPlayerPosition = target.position;
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void SeePlayer()
    {
        CanSeePlayer = true;
        LastPlayerPosition = _playerRef.transform.position;
    }
}
