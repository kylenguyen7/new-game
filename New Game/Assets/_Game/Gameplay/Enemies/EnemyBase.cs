using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : MonoBehaviour {
    [SerializeField] private int _hp = 10;
    
    // Taking damage
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _initialColor = Color.white;
    [SerializeField] private Color _damagedColor = Color.red;
    [SerializeField] private float _damagedColorFadeTime = 0.25f;
    private Coroutine _damageCouritine;
    
    // Dying
    [SerializeField] private GameObject _deathParticlesPrefab;
    [SerializeField] private GameObject _itemPrefab;

    protected void Awake() {
        _spriteRenderer.color = _initialColor;
    }

    public void TakeDamage() {
        if (_damageCouritine != null) {
            StopCoroutine(_damageCouritine);
        }
        _damageCouritine = StartCoroutine(DamageCoroutine());
    }
    
    private void DestroyMe() {
        SpawnItem();
        SpawnDeathEffects();
        Destroy(gameObject);
    }

    private void SpawnItem() {
        ItemController item = Instantiate(_itemPrefab, transform.position, Quaternion.identity)
            .GetComponent<ItemController>();
        item.Scatter();
    }

    private void SpawnDeathEffects() {
        Instantiate(_deathParticlesPrefab, transform.position, Quaternion.identity);
    }
    
    private IEnumerator DamageCoroutine() {
        _hp--;
        if (_hp <= 0) {
            DestroyMe();
            yield break;
        }
        
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
