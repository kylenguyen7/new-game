using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Handles overall movement of a spear, i.e. by providing methods for
 * initializing its direction and force. Attached to the same object as the
 * rigidbody for the spear.
 *
 * Brief description of spear part responsibilities:
 *
 * - Spear: Controls movement of spear and auto-destroy
 * - LineConnection: Controls drawing of line and calculation of pull
 * - Barb: Controls collisions
 * 
 */
[RequireComponent(typeof(Rigidbody2D))]
public class SpearController : MonoBehaviour {
    [SerializeField] private float _initialForce;
    [SerializeField] private float _despawnTime;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpearBarbController _spearBarbController;
    
    private Rigidbody2D _rb;
    private FixedJoint2D _fixedJoint2D;
    private Vector2 _heading;
    private Transform creatorTransform;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _fixedJoint2D = GetComponent<FixedJoint2D>();
    }

    private void Start() {
        _spearBarbController.onHitSomething += HandlePullableCollision;
        Invoke("DestroyMe", _despawnTime);
    }

    private void HandlePullableCollision(Pullable pullable) {
        _rb.velocity = Vector2.zero;
        _rb.mass = 0;
        _fixedJoint2D.enabled = true;
        _fixedJoint2D.connectedBody = pullable.gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer.sortingOrder = -1;
        CancelInvoke("DestroyMe");
    }

    public void Init(Vector2 initialDirection) {
        _heading = initialDirection.normalized;
        transform.right = _heading;
        _rb.AddForce(_heading * _initialForce);
    }

    private void DestroyMe() {
        Destroy(gameObject);
    }
}
