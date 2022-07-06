using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ColorFlashDamageable : Damageable {
    // Taking damage
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    private Color _initialColor = Color.white;
    private Color _damagedColor = Color.red;
    private float _damagedColorFadeTime = 0.25f;
    
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
        _damageCoroutine = null;
    }
}
