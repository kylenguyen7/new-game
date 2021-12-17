using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShurikenController : MonoBehaviour {
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _initialSpeed;
    [SerializeField] private GameObject _deathParticlesPrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _destorySelfTime;
    
    public void Init(Vector2 initialDirection) {
        var dir = initialDirection.normalized;
        transform.right = dir;
        _rb.velocity = dir * _initialSpeed;
        Invoke(nameof(DestroyMe), _destorySelfTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(_damage, Vector2.zero, 0);
            DestroyMe();
        }

        if (other.CompareTag("Bouncy")) {
            DestroyMe();
        }
    }

    private void DestroyMe() {
        Instantiate(_deathParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
