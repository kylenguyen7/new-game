using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearItemStickyController : MonoBehaviour {
    [SerializeField] private Rigidbody2D _rb;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == ApothecaryConstants.LAYER_ITEMS && _rb.velocity.magnitude > 0) {
            ItemController item = other.GetComponent<ItemController>();
            item.Attach(_rb);
        }
    }
}
