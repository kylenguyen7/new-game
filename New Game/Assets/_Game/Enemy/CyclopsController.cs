using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CyclopsController : MonoBehaviour {
    private Vector2 movement;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private int _hp;
    
    // Taking damage
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _initialColor;
    [SerializeField] private Color _damagedColor;
    [SerializeField] private float _damagedColorFadeTime;
    private Coroutine _damageCouritine;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer.color = _initialColor;
    }

    private void Update() {
        movement = new Vector2(
            (Input.GetKey(KeyCode.L) ? 1 : 0) - (Input.GetKey(KeyCode.J) ? 1 : 0),
            (Input.GetKey(KeyCode.I) ? 1 : 0) - (Input.GetKey(KeyCode.K) ? 1 : 0));
        movement.Normalize();
    }

    private void FixedUpdate() {
        _rb.velocity = movement * _speed; // + CalculatePull();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log($"Hit by {other.gameObject.name}");
    }

    public void TakeDamage() {
        if (_damageCouritine != null) {
            StopCoroutine(_damageCouritine);
        }
        _damageCouritine = StartCoroutine(DamageCoroutine());
    }
    
    private IEnumerator DamageCoroutine() {
        _hp--;
        if (_hp <= 0) {
            Destroy(gameObject);
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
