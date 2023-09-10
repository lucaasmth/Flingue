using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask enemyMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleEnemies = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;

    public MeshFilter viewMeshFilter;
    
    private Mesh _viewMesh;

    private void Start()
    {
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = _viewMesh;
        StartCoroutine(nameof(FindEnemiesWithDelay), .2f);
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private IEnumerator FindEnemiesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleEnemies();
        }
    }

    private void FindVisibleEnemies()
    {
        visibleEnemies.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
        foreach (Collider enemy in targetsInViewRadius)
        {
            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            if (!(Vector3.Angle(transform.forward, dirToEnemy) < viewAngle / 2)) continue;
            float dstToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (!Physics.Raycast(transform.position, dirToEnemy, dstToEnemy, obstacleMask))
            {
                // No obstacle in the way
                visibleEnemies.Add(enemy.transform);
            }
        }
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                if (oldViewCast.Hit != newViewCast.Hit)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero) viewPoints.Add(edge.pointA);
                    if(edge.pointB != Vector3.zero) viewPoints.Add(edge.pointB);
                }
            }
            
            viewPoints.Add(newViewCast.Point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i >= vertexCount - 2) continue;
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.Angle;
        float maxAngle = maxViewCast.Angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (newViewCast.Hit == minViewCast.Hit)
            {
                minAngle = angle;
                minPoint = newViewCast.Point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.Point;
            }

        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        if (Physics.Raycast(transform.position, dir, out var hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private struct ViewCastInfo
    {
        public bool Hit;
        public Vector3 Point;
        public float Dst;
        public float Angle;

        public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
        {
            Hit = hit;
            Point = point;
            Dst = dst;
            Angle = angle;
        }
    }

    private struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}
