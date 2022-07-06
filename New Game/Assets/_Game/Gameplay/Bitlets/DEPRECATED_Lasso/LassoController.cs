using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoController : MonoBehaviour {
    [SerializeField] private float initialSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private float returnSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lineEndOffset;
    
    private Vector2 initialDir;
    private bool returning;
    private Transform origin;
    
    public void Init(Vector2 direction, Transform origin) {
        direction = direction.normalized;
        rb.velocity = direction * initialSpeed;
        initialDir = direction;
        this.origin = origin;
    }

    private void FixedUpdate() {
        if (returning) { 
            rb.velocity = (origin.position - transform.position).normalized * returnSpeed;
        } else {
            rb.velocity += (Vector2)(deceleration * (origin.position - transform.position).normalized);
            if (Vector2.Dot(initialDir, rb.velocity) < 0) {
                returning = true;
            }
        }
    }

    private void Update() {
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, transform.position + (origin.position - transform.position).normalized * lineEndOffset);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (returning && other.CompareTag("Player")) {
            Destroy(gameObject);
        }

        if (other.CompareTag("Bitlet")) {
            var bitlet = other.GetComponent<BitletController>();
            bitlet.AttachRope(origin);
            Destroy(gameObject);
        }
    }
}
