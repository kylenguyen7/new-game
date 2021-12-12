using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : Damageable {
    // Taking damage
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _initialColor = Color.white;
    [SerializeField] private Color _damagedColor = Color.red;
    [SerializeField] private float _damagedColorFadeTime = 0.25f;

    protected new void Awake() {
        base.Awake();
        _spriteRenderer.color = _initialColor;
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
