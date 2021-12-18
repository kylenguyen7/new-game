using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEngine;

public class Rotator : MonoBehaviour {
    // TOOD: Make this class static
    public static Rotator instance;
    [SerializeField, Range(0, 1)] private float _rotationMag;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    /**
     * Example usage:
     * 
     * private void FixedUpdate() {
     *     transform.right = Rotator.instance.UpdateHeading(transform.right);
     * }
     */
    public Vector2 UpdateHeading(Vector2 heading, Vector2 position) {
        heading.Normalize();
        Vector2 toMouse = KaleUtils.GetMousePosWorldCoordinates() - position;
        
        int sign = -Math.Sign(Vector3.Cross(toMouse, heading).z);
        float angle = (float)Math.Acos(Vector2.Dot(toMouse, heading) / toMouse.magnitude) * 180 / (float)Math.PI;
        if (angle.Equals(Single.NaN)) {
            angle = 0;
        }
        float rotation = angle * sign * _rotationMag;
        return Quaternion.AngleAxis(rotation, Vector3.forward) * heading;
    }
}
