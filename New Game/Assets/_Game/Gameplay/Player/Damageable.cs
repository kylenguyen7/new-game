using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Damageable : MonoBehaviour {
    [SerializeField] private float _hp;
    private Vector2 _knockback;
    private Coroutine _damageCoroutine;
    private Rigidbody2D _rb;

    [SerializeField] float knockbackDecayPerFrame;
    
    public Vector2 Velocity { get; set; }
    
    protected void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected void Update() {
        DecayKnockback();
    }

    protected void FixedUpdate() {
        _rb.velocity = Velocity + _knockback;
    }

    public void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude) {
        DepleteHealth(damage);
        AddKnockback(kbDirection, kbMagnitude);
        
        if (_damageCoroutine != null) {
            StopCoroutine(_damageCoroutine);
        }
        _damageCoroutine = StartCoroutine(DamageCoroutine());
    }

    private void DepleteHealth(float damage) {
        _hp -= damage;
        if (_hp <= 0) {
            Die();
        }
    }
    
    private void AddKnockback(Vector2 kbDirection, float kbMagnitude) {
        _knockback += kbDirection.normalized * kbMagnitude;
    }

    /**
     * Can be made into protected virtual function if some Damageables want to decay
     * knockback differently.
     */
    private void DecayKnockback() {
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, knockbackDecayPerFrame * Time.deltaTime);
    }

    protected abstract void Die();
    protected abstract IEnumerator DamageCoroutine();
}
