using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Die()
    {
        IsDead = true;
        FieldOfView fov = GetComponent<FieldOfView>();
        fov.viewAngle = 360;
        fov.viewRadius = 12f;
    }
}
