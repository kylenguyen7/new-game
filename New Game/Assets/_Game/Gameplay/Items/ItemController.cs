using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FixedJoint2D))]
public class ItemController : MonoBehaviour {
    private Rigidbody2D _rb;
    private FixedJoint2D _fixedJoint2D;
    private Collider2D _collider;
    [SerializeField] private float _scatterForce;
    private bool _attached;
    
    // Sprite
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Item _item;
    
    // Picking up
    [SerializeField] private float _phaseTime;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(_scatterForce * Random.insideUnitCircle);
        _fixedJoint2D = GetComponent<FixedJoint2D>();
        
        gameObject.layer = ApothecaryConstants.LAYER_PHASING;
        Invoke(nameof(MoveToItemLayer), _phaseTime);
        _spriteRenderer.sprite = _item.Sprite;
    }

    private void MoveToItemLayer() {
        gameObject.layer = ApothecaryConstants.LAYER_ITEMS;
    }

    public void Attach(Rigidbody2D target) {
        if (_attached) return;

        _collider.isTrigger = true;
        _attached = true;
        DisableRigidbody();
        _fixedJoint2D.enabled = true;
        _fixedJoint2D.connectedBody = target;
    }

    private void DisableRigidbody() {
        _rb.mass = 0;
        _rb.drag = 0;
        _rb.angularDrag = 0;
    }
    
    // TODO: Rely only on collisions, not triggers
    // (right now triggers are used as a way to easily phase after being speared; see line 41)
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PickUp();
        }
        _rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PickUp();
        }
    }

    private void PickUp() {
        Inventory.Instance.AddItem(_item.Name, 1);
        Destroy(gameObject);
    }
}
