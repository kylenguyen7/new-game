using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashBugTestController : MonoBehaviour {
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    private float _dashTimer;
    private bool dashing = false;
    private Rigidbody2D _rb;
    private bool _dashQueued;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(!dashing && Input.GetKeyDown(KeyCode.LeftShift)) {
            dashing = true;
            _rb.velocity = Vector2.right * _dashSpeed;
            _dashTimer = _dashTime;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            transform.position = Vector2.zero;
        }
        
        if (dashing && _dashTimer <= 0) {
            dashing = false;
            _rb.velocity = Vector2.zero;
        }
        _dashTimer -= Time.deltaTime;
    }

    private void FixedUpdate() {
        
    }
}
