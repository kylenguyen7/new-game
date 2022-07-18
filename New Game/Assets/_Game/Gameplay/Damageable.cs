using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Damageable : MonoBehaviour {
    // Delegate specifies a function prototype (here it is a void func with no parameters)
    public delegate void OnDeath();
    // Event specifies a subscription list of OnDeath type functions to call when death occurs
    public event OnDeath OnDeathCallback;

    public delegate void OnDamaged();

    public event OnDamaged OnDamagedCallback;
    
    [SerializeField] private float _hp;
    private Vector2 _knockback;
    protected Coroutine _damageCoroutine;
    protected Rigidbody2D _rb;
    
    // Unique damage source enforcement
    private HashSet<String> previousDamageSourceUuids;
    
    public float StartingHp { private set; get; }
    public float Hp => _hp;
    
    // Dying
    [SerializeField] private GameObject _deathParticlesPrefab;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Item _itemScriptableObject;
    [SerializeField] float _knockbackDecay;
    private bool Dead;

    public Vector2 Velocity { get; set; }
    
    protected void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        previousDamageSourceUuids = new HashSet<String>();
        StartingHp = _hp;
    }

    protected void Update() { }

    protected void FixedUpdate() {
        DecayKnockback();
        _rb.velocity = Velocity + _knockback;
    }

    public virtual void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude, String uuid) {
        if (previousDamageSourceUuids.Contains(uuid)) return;
        previousDamageSourceUuids.Add(uuid);
        
        OnDamagedCallback?.Invoke();
        DepleteHealth(damage);
        AddKnockback(kbDirection, kbMagnitude);

        // Start damage coroutine
        if (_damageCoroutine != null) {
            StopCoroutine(_damageCoroutine);
        }

        var coroutine = DamageCoroutine();
        if (coroutine != null) {
            _damageCoroutine = StartCoroutine(coroutine);
        }
    }
    
    // BEGIN PRIVATE HELPER FUNCTIONS
    private void DepleteHealth(float damage) {
        _hp -= damage;
        if (_hp <= 0) {
            Die();
        }
    }

    protected abstract IEnumerator DamageCoroutine();

    #region Dying
    private void SpawnItem() {
        if (_itemPrefab == null) return;
        
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity).GetComponent<ItemController>();
        item.Init(_itemScriptableObject);
    }

    private void SpawnDeathEffects() {
        if (_deathParticlesPrefab != null) {
            Instantiate(_deathParticlesPrefab, transform.position, Quaternion.identity);
        }
    }

    protected void Die() {
        if (Dead) return;
        Dead = true;
        if (_itemPrefab) SpawnItem();
        SpawnDeathEffects();
        OnDeathCallback?.Invoke();
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
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, _knockbackDecay);
    }
    #endregion
}
