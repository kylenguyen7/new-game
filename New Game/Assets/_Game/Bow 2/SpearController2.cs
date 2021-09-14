using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpearController2 : MonoBehaviour {
    // Movement
    [SerializeField] private float _initialForce;
    [SerializeField] private float _retractionSpeed;
    private Rigidbody2D _rb;

    // Retraction
    private Transform _owner;
    private bool retracted = false;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1) && !retracted) {
            retracted = true;
        }
    }

    private void FixedUpdate() {
        if (retracted) {
            Vector2 toOwner = (_owner.position - transform.position).normalized;
            transform.right = toOwner;
            _rb.velocity = toOwner * _retractionSpeed;
        }
    }

    public void Init(Vector2 initialDirection, Transform owner) {
        var dir = initialDirection.normalized;
        transform.right = dir;
        _rb.AddForce(dir * _initialForce);

        _owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (retracted && other.CompareTag("Player")) {
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy")) {
            CyclopsController _enemy = other.GetComponent<CyclopsController>();
            _enemy.TakeDamage();
        }
    }
}