using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected bool rotate;

    protected void rotate_towards(Vector2 dir) {
        float angle = Vector2.Angle(Vector2.up, dir);
        Vector3 cross = Vector3.Cross(Vector2.up, dir);
        if (cross.z > 0) {
            angle = 360 - angle;
        }
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);
    }

    public abstract void set_target(GameObject target);
}
