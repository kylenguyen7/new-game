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
    [SerializeField] private GameObject _spearTip;
    
    // Destroying
    public delegate void OnDestroySpear();

    public event OnDestroySpear OnDestroySpearCallback;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        GestureDetector.instance.OnGestureCallback += Redirect;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !retracted) {
            _spearTip.SetActive(false);
            retracted = true;
        }
    }

    private void OnDisable() {
        GestureDetector.instance.OnGestureCallback -= Redirect;
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
        SetVelocity(dir * _initialSpeed);
        _owner = owner;
    }

    private void Redirect(Vector2 direction) {
        SetVelocity(direction.normalized * _initialSpeed);
    }

    private void SetVelocity(Vector2 velocity) {
        _rb.velocity = velocity;
        transform.right = velocity.normalized;
    }

    private void OnDestroy() {
        OnDestroySpearCallback?.Invoke();
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
        if (other.gameObject.CompareTag("Bouncy") || other.gameObject.CompareTag("Enemy Bouncy")) {
            float bounciness = other.gameObject.GetComponent<TerrainController>().Bounciness;
            if (bounciness <= 0) {
                _rb.velocity = Vector2.zero;
                return;
            }
            
            Vector2 normal = other.GetContact(0).normal;
            // Component of velocity in direction of normal
            Vector2 velocityNormalComp = normal * Vector2.Dot(normal, _previousVelocity);
            Vector2 newVelocity = _previousVelocity - 2 * velocityNormalComp;
            SetVelocity(newVelocity * bounciness);
        }
    }
}