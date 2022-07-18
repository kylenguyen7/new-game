using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpearController : MonoBehaviour {
    // Dealing damage
    [SerializeField] private float _damage;
    [SerializeField] private float _kbMagnitude;
    [SerializeField] private GameObject hitEffect;
    
    // Movement
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _retractionSpeed;
    private Rigidbody2D _rb;

    // Retraction
    private Transform _owner;
    private bool retracted;
    
    // Bouncing
    private Vector2 _previousVelocity = Vector2.zero;
    [SerializeField] private GameObject _spearTip;
    
    // Destroying
    public delegate void OnDestroySpear();
    public event OnDestroySpear OnDestroySpearCallback;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !retracted) {
            _spearTip.SetActive(false);
            retracted = true;
        }
    }

    private void FixedUpdate() {
        if (retracted) {
            Vector2 to = (_owner.position - transform.position).normalized;
            transform.right = to;
            _rb.velocity = to * _retractionSpeed;
        }
        
        _previousVelocity = _rb.velocity;
    }

    public void Init(Vector2 initialDirection, Transform owner) {
        var dir = initialDirection.normalized;
        SetVelocity(dir * _initialSpeed);
        _owner = owner;
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

        if (other.gameObject.layer == ApothecaryConstants.LAYER_ENEMIES) {
            ColorFlashDamageable enemy = other.GetComponent<ColorFlashDamageable>();
            enemy.TakeDamage(_damage, transform.right, _kbMagnitude, Guid.NewGuid().ToString());
            
            Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == ApothecaryConstants.LAYER_TERRAIN || other.gameObject.layer == ApothecaryConstants.LAYER_ENEMIES) {
            //
            _rb.velocity = Vector2.zero;
            return;
            //
            
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