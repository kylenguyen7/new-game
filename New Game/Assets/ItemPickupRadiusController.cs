using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupRadiusController : MonoBehaviour {
    private PlayerController _player;
    public PlayerController PlayerInRadius => _player;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _player = other.gameObject.GetComponent<PlayerController>();
        }
    }
}
