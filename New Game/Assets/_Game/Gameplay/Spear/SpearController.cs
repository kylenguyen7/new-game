using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpearController : MonoBehaviour {
    // Dealing damage
    [SerializeField] private float _damage;
    [SerializeField] private float _kbMagnitude;
    
    // Movement
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _retractionSpeed;
    private Rigidbody2D _rb;

    // Retraction
    private Transform _owner;
    private bool retracted = false;
    
    // Bouncing
    private Vector2 _previousVelocity = Vector2.zero;
    [SerializeField] private float _floatDecayFactor;
    [SerializeField] private GameObject _spearTip;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1) && !retracted) {
            retracted = true;
            _spearTip.SetActive(false);
        }
    }

    private void FixedUpdate() {
        if (retracted) {
            Vector2 toOwner = (_owner.position - transform.position).normalized;
            transform.right = toOwner;
            _rb.velocity = toOwner * _retractionSpeed;
        }

        _previousVelocity = _rb.velocity;
    }

    public void Init(Vector2 initialDirection, Transform owner) {
        var dir = initialDirection.normalized;
        SetFacing(dir);
        _rb.velocity = dir * _initialSpeed;
        _owner = owner;
    }
    
    // Direction the spear is pointing
    private void SetFacing(Vector2 facing) {
        transform.right = facing;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (retracted && other.CompareTag("Player")) {
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy")) {
            Damageable enemy = other.GetComponent<Damageable>();
            enemy.TakeDamage(_damage, transform.right, _kbMagnitude);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Bouncy")) {
            Vector2 normal = other.GetContact(0).normal;
            // Component of velocity in direction of normal
            Vector2 velocityNormalComp = normal * Vector2.Dot(normal, _previousVelocity);
            Vector2 newVelocity = _previousVelocity - 2 * velocityNormalComp;
            
            _rb.velocity = newVelocity * _floatDecayFactor;
            SetFacing(newVelocity.normalized);
        }
    }
}