using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEngine;

public static class Rotator  {
    /**
     * Example usage:
     * 
     * private void FixedUpdate() {
     *     transform.right = Rotator.instance.UpdateHeading(transform.right);
     * }
     */
    public static Vector2 UpdateHeading(Vector2 from, Vector2 to, float t) {
        from.Normalize();
        to.Normalize();
        int sign = Math.Sign(Vector3.Cross(from, to).z);
        float angle = (float)Math.Acos(Vector2.Dot(from, to)) * 180 / (float)Math.PI;
        if (angle.Equals(Single.NaN)) {
            angle = 0;
        }
        float rotation = angle * sign * t;
        return Quaternion.AngleAxis(rotation, Vector3.forward) * from;
    }
}
