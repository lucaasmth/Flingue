using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EnemyFieldOfView))]
    public class EnemyFieldOfViewEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            EnemyFieldOfView enemyFieldOfView = (EnemyFieldOfView)target;
            Vector3 transformPosition = enemyFieldOfView.transform.position;
            Handles.color = Color.white;
            Handles.DrawWireArc(transformPosition, Vector3.up, Vector3.forward, 360, enemyFieldOfView.radius);
            
            Vector3 viewAngleA = enemyFieldOfView.DirFromAngle(-enemyFieldOfView.angle / 2, false);
            Vector3 viewAngleB = enemyFieldOfView.DirFromAngle(enemyFieldOfView.angle / 2, false);
            Handles.DrawLine(transformPosition, transformPosition + viewAngleA * enemyFieldOfView.radius);
            Handles.DrawLine(transformPosition, transformPosition + viewAngleB * enemyFieldOfView.radius);
        }
    }
}
