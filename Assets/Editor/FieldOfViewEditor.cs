using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;
            Handles.color = Color.white;
            Vector3 transformPosition = fov.transform.position;
            Handles.DrawWireArc(transformPosition, Vector3.up, Vector3.forward, 360, fov.viewRadius);
            Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
            Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);
            Handles.DrawLine(transformPosition, transformPosition + viewAngleA * fov.viewRadius);
            Handles.DrawLine(transformPosition, transformPosition + viewAngleB * fov.viewRadius);

            Handles.color = Color.red;
            foreach (Transform visibleEnemy in fov.visibleEnemies)
            {
                Handles.DrawLine(fov.transform.position, visibleEnemy.position);
            }
        }
    }
}