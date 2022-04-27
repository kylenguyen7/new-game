using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteShake : MonoBehaviour {
    private Vector2 _startingPos;
    private Vector2 _currentPos;
    private bool _shaking;
    [SerializeField] private float shakeFreq;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDecay;

    public void StartShake() {
        _shaking = true;
    }

    public void PauseShake() {
        _shaking = false;
    }
    
    private void Awake() {
        InvokeRepeating(nameof(AddShake), 0f, shakeFreq);
    }

    private void Update() {
        if (_shaking) {
            _currentPos = Vector2.Lerp(_currentPos, Vector2.zero, shakeDecay);
            transform.localPosition = _currentPos;
        } else {
            transform.localPosition = Vector2.zero;
        }
        Debug.Log(_shaking);
    }

    private void AddShake() {
        if (!_shaking) return;
        float x = (Random.value < 0.5f ? -1 : 1) * Random.Range(0.5f, 1f) * shakeMagnitude;
        float y = (Random.value < 0.5f ? -1 : 1) * Random.Range(0.5f, 1f) * shakeMagnitude;
        _currentPos = _startingPos + new Vector2(x, y);
    }
}
