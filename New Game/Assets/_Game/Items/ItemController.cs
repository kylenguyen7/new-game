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
    [SerializeField] private float scatterForce;
    private bool _attached;
    
    // Sprite
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _sprites;
    
    // Picking up
    [SerializeField] private Collider2D _itemCollider;
    [SerializeField] private float _colliderEnableDelay;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _fixedJoint2D = GetComponent<FixedJoint2D>();
        _itemCollider.enabled = false;
        Invoke(nameof(EnableCollider), _colliderEnableDelay);

        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Count - 1)];
    }

    private void EnableCollider() {
        _itemCollider.enabled = true;
    }

    public void Scatter() {
        _rb.AddForce(scatterForce * Random.insideUnitCircle);
    }

    public void Attach(Rigidbody2D target) {
        if (_attached) return;
        
        _attached = true;
        _fixedJoint2D.enabled = true;
        _fixedJoint2D.connectedBody = target;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("PIcked up an item!");
            Destroy(gameObject);
        }
    }
}
