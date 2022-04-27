using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FixedJoint2D))]
public class ItemController : MonoBehaviour {
    private Rigidbody2D _rb;
    [SerializeField] private float _scatterForce;
    private bool _canBePickedUp;
    private Item _item;

    
    // Picking up
    [SerializeField] private float _phaseTime;
    [SerializeField] private ItemPickupRadiusController radius;
    [SerializeField] private float magnetAcceleration;
    [SerializeField] private SpriteRenderer sprite;
    
    // SPEARING
    private FixedJoint2D _fixedJoint2D;
    private bool _attached;
    private Collider2D _collider;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(_scatterForce * Random.insideUnitCircle);
        _fixedJoint2D = GetComponent<FixedJoint2D>();
        
        gameObject.layer = ApothecaryConstants.LAYER_PHASING;
        Invoke(nameof(MoveToItemLayer), _phaseTime);
    }

    private void Update() {
        if (_canBePickedUp && radius.PlayerInRadius != null) {
            _rb.AddForce((radius.PlayerInRadius.transform.position - transform.position).normalized * magnetAcceleration);
            float scale = (Vector2.Distance(transform.position, radius.PlayerInRadius.transform.position) + 0.2f) / 1.5f;
            scale = Mathf.Min(scale, 1f);
            sprite.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    public void Init(Item item) {
        _item = item;
        GetComponentInChildren<SpriteRenderer>().sprite = _item.Sprite;
    }

    private void MoveToItemLayer() {
        _canBePickedUp = true;
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
    
    // DISABLED ALONG WITH SPEAR BEHAVIOR
    // private void OnCollisionEnter2D(Collision2D other) {
    //     if (_canBePickedUp && other.gameObject.CompareTag("Player")) {
    //         PickUp();
    //     }
    //     _rb.velocity = Vector2.zero;
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        if (_canBePickedUp && other.gameObject.CompareTag("Player")) {
            PickUp();
        }
    }

    private void PickUp() {
        Inventory.Instance.TryAddOne(_item);
        Destroy(gameObject);
    }
}
