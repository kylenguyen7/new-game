

using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ParabolicProjectileController : MonoBehaviour {
    // Timing
    [SerializeField] private float travelTime;
    private float _timer;
    
    // Movement
    private Rigidbody2D _rb;
    private Transform _targetTransform;
    private Vector2 _targetPosition;
    private bool _targetSet;
    
    // Height illusion
    [SerializeField] private GameObject sprite;
    [SerializeField] private float heightMagnitude;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected void SetTarget(Transform target) {
        if (_targetSet) return;
        _targetTransform = target;
        _targetSet = true;
    }

    protected void SetTarget(Vector2 position) {
        if (_targetSet) return;
        _targetPosition = position;
        _targetSet = true;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, _targetPosition);
    }
    
    private void FixedUpdate() {
        if (!_targetSet) return;
        
        if (_timer >= travelTime) {
            OnReachDestination();
            return;
        }
        
        if (_targetTransform != null) {
            _targetPosition = _targetTransform.position;
        }
        _rb.velocity = (_targetPosition - (Vector2) transform.position) / (travelTime - _timer);
        sprite.transform.localPosition = new Vector3(0f, heightMagnitude * (-Mathf.Pow(_timer, 2) + _timer * travelTime), 0f);
        _timer = Mathf.Min(_timer + Time.deltaTime, travelTime);
    }

    protected abstract void OnReachDestination();
}