using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Damageable : MonoBehaviour {
    [SerializeField] private float _hp;
    private Vector2 _knockback;
    private Coroutine _damageCoroutine;
    protected Rigidbody2D _rb;
    
    // Dying
    [SerializeField] private GameObject _deathParticlesPrefab;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] float knockbackDecayPerFrame;
    
    // Taking damage
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    private Color _initialColor = Color.white;
    private Color _damagedColor = Color.red;
    private float _damagedColorFadeTime = 0.25f;
    
    public Vector2 Velocity { get; set; }
    
    protected void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer.color = _initialColor;
    }

    protected void Update() {
        DecayKnockback();
    }

    protected void FixedUpdate() {
        // Removed knockback for now, since I don't think it feels good right now
        _rb.velocity = Velocity; // + _knockback;
    }

    public virtual void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude) {
        DepleteHealth(damage);
        AddKnockback(kbDirection, kbMagnitude);

        // Start damage coroutine
        if (_damageCoroutine != null) {
            StopCoroutine(_damageCoroutine);
        }
        _damageCoroutine = StartCoroutine(DamageCoroutine());
    }
    
    // BEGIN PRIVATE HELPER FUNCTIONS
    private void DepleteHealth(float damage) {
        _hp -= damage;
        if (_hp <= 0) {
            Die();
        }
    }
    
    private IEnumerator DamageCoroutine() {
        _spriteRenderer.color = _damagedColor;
        float time = 0;
        float lastTime = Time.time;
        yield return new WaitForEndOfFrame();
        
        while (time < _damagedColorFadeTime) {
            time += (Time.time - lastTime);
            lastTime = Time.time;
            
            _spriteRenderer.color = Color.Lerp(_damagedColor, _initialColor, time / _damagedColorFadeTime);
            yield return new WaitForEndOfFrame();
        }
        _spriteRenderer.color = _initialColor;
    }

    #region Dying
    private void SpawnItem() {
        ItemController item = Instantiate(_itemPrefab, transform.position, Quaternion.identity)
            .GetComponent<ItemController>();
        item.Scatter();
    }

    private void SpawnDeathEffects() {
        Instantiate(_deathParticlesPrefab, transform.position, Quaternion.identity);
    }

    private void Die() {
        if (_itemPrefab) SpawnItem();
        SpawnDeathEffects();
        Destroy(gameObject);
    }
    #endregion
    
    #region Knockback
    private void AddKnockback(Vector2 kbDirection, float kbMagnitude) {
        _knockback += kbDirection.normalized * kbMagnitude;
    }

    /**
     * Right now, all Damageables decay knockback in the same way.
     * Can be made into protected virtual function if some Damageables want to decay
     * knockback differently.
     */
    private void DecayKnockback() {
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, knockbackDecayPerFrame * Time.deltaTime);
    }
    #endregion
}
