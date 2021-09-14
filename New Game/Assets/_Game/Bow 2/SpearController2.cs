using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpearController2 : MonoBehaviour {
    [SerializeField] private float _initialForce;
    private Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 initialDirection) {
        var dir = initialDirection.normalized;
        transform.right = dir;
        _rb.AddForce(dir * _initialForce);
    }
}