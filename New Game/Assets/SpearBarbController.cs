using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Controls the spear barb, providing collision information once meeting a target
 */
[RequireComponent(typeof(Collider2D))]
public class SpearBarbController : MonoBehaviour {
    [SerializeField] private SpearController _spearController;
    [SerializeField] private LayerMask _targetLayerMask;

    public Action<Rigidbody2D> onHitSomething;

    private Collider2D _collider;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Ignore trigger enters from objects not in layer mask
        if ((1 << other.gameObject.layer & _targetLayerMask) == 0) {
            return;
        }
        
        Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
        
        if (onHitSomething != null) {
            onHitSomething.Invoke(hit);
        }
    }
}
