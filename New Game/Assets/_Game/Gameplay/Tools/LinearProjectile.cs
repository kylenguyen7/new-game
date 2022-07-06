using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class LinearProjectile : MonoBehaviour {
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _destorySelfTime;
    
    public void Init(Vector2 initialDirection) {
        var dir = initialDirection.normalized;
        transform.right = dir;
        _rb.velocity = dir * _initialSpeed;
        Invoke(nameof(DestroyMe), _destorySelfTime);
    }

    protected abstract void OnTriggerEnter2D(Collider2D other);
    
    protected void DestroyMe() {
        Destroy(gameObject);
    }
}