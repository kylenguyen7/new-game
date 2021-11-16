using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a rope between two Pullable objects.
 * Once attached, a pull can be calculated from either of the Pullables.
 */
public class RopeController : MonoBehaviour {
    private RopeConnection Origin { get; set; }
    private RopeConnection Target { get; set; }

    [SerializeField] private float _range;
    [SerializeField] private float _pullMultiplier;
    private bool _attached;

    protected void Start() {
        Target = new RopeConnection(transform, null);
    }

    void Update() {
        if (Origin != null && Target != null) {
            Vector2 targetToOrigin = Target.Transform.position - Origin.Transform.position;
            Debug.DrawLine(Origin.Transform.position, Target.Transform.position,
                Color.Lerp(Color.white, Color.red, CalculateRawPull(targetToOrigin).magnitude / 8f));
        }
    }
    
    /**
     * Calculates the pull on a Pullable object, which must be either the origin
     * or the target of this RopeController. If not yet attached, this returns Vector2.zero.
     */
    public Vector2 GetPull(Pullable pullable) {
        if (pullable != Origin.Pullable && pullable != Target.Pullable) {
            Debug.LogWarning("Attempted to get pull for an unrelated RopeController.");
            return Vector2.zero;
        }
        
        if (!_attached) {
            return Vector2.zero;
        }

        if (Origin.Pullable == pullable) {
            Vector2 targetToOrigin = Target.Transform.position - Origin.Transform.position;
            return CalculateRawPull(targetToOrigin) / pullable.PullPower;
        } else {
            Vector2 originToTarget = Origin.Transform.position - Target.Transform.position;
            return CalculateRawPull(originToTarget) / pullable.PullPower;
        }
    }

    /**
     * Sets the origin of the RopeController. Pullable may be null, in which
     * case the rope will not pull on either end.
     */
    public void Init(Transform transform, Pullable pullable) {
        Origin = new RopeConnection(transform, pullable);
        pullable.AddRope(this);
    }
    
    public void Attach(Pullable pullable) {
        _attached = true;
        Target = new RopeConnection(Target.Transform, pullable);
        pullable.AddRope(this);
    }

    private Vector2 CalculateRawPull(Vector2 direction) {
        if (direction.magnitude > _range) {
            float pullMagnitude = (direction.magnitude - _range) * _pullMultiplier;
            return direction.normalized * pullMagnitude;
        }

        return Vector2.zero;
    }

    private void OnDestroy() {
        Origin.Pullable?.RemoveRope(this);
        Target.Pullable?.RemoveRope(this);
    }

    /**
     * Internal class for wrapping a transform and Pullable.
     */
    [Serializable]
    private class RopeConnection {
        public Transform Transform { get; }
        public Pullable Pullable { get; }
    
        public RopeConnection(Transform transform, Pullable pullable) {
            if (transform == null) {
                Debug.LogError("Attempted to create a RopeConnection without a transform.");
            }

            Transform = transform;
            Pullable = pullable;
        }
    }
}