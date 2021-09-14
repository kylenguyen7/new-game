using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsController : MonoBehaviour {
    private Vector2 movement;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movement = new Vector2(
            (Input.GetKey(KeyCode.L) ? 1 : 0) - (Input.GetKey(KeyCode.J) ? 1 : 0),
            (Input.GetKey(KeyCode.I) ? 1 : 0) - (Input.GetKey(KeyCode.K) ? 1 : 0));
        movement.Normalize();
    }

    private void FixedUpdate() {
        _rb.velocity = movement * _speed; // + CalculatePull();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log($"Hit by {other.gameObject.name}");
    }
}
