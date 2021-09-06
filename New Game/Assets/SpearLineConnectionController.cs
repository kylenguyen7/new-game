using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearLineConnectionController : MonoBehaviour {
    [SerializeField] private SpearBarbController _spearBarbController;
    [SerializeField] float _range;
    [SerializeField] private float _pullMultiplier;
    [SerializeField] private float _maxPullMagnitude;
    
    public Vector2 Pull => _pullDir * _pullMagnitude;

    private Transform _origin;
    private bool _hitDetected;
    
    private float _pullMagnitude = 0;
    private Vector2 _pullDir = Vector2.zero;

    private void OnEnable() {
        _spearBarbController.onHitSomething += Hit;
    }

    private void OnDisable() {
        _spearBarbController.onHitSomething += Hit;
    }

    private void Update() {
        if (_hitDetected) {
            Vector2 positionToOrigin = transform.position - _origin.position;
            if (positionToOrigin.magnitude > _range) {
                _pullMagnitude = (positionToOrigin.magnitude - _range) * _pullMultiplier;
                _pullDir = positionToOrigin.normalized;
            }
        }
        
        Debug.DrawLine(_origin.position, transform.position, Color.Lerp(Color.white, Color.red, _pullMagnitude / _maxPullMagnitude));
    }

    public void Init(Transform creator) {
        Debug.Log("Initialized! My origin is " + creator);
        _origin = creator;
    }
    
    private void Hit(Rigidbody2D hit) {
        _hitDetected = true;
    }
}
