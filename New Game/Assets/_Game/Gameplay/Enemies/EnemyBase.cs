using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : Damageable {
    // Taking damage
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _initialColor = Color.white;
    [SerializeField] private Color _damagedColor = Color.red;
    [SerializeField] private float _damagedColorFadeTime = 0.25f;

    // Dying
    [SerializeField] private GameObject _deathParticlesPrefab;
    [SerializeField] private GameObject _itemPrefab;

    protected new void Awake() {
        base.Awake();
        _spriteRenderer.color = _initialColor;
    }

    private void SpawnItem() {
        ItemController item = Instantiate(_itemPrefab, transform.position, Quaternion.identity)
            .GetComponent<ItemController>();
        item.Scatter();
    }

    private void SpawnDeathEffects() {
        Instantiate(_deathParticlesPrefab, transform.position, Quaternion.identity);
    }

    protected override void Die() {
        SpawnItem();
        SpawnDeathEffects();
        Destroy(gameObject);
    }

    protected override IEnumerator DamageCoroutine() {
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
}
