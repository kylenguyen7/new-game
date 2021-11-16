using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DasherStatePrepare : IState {
    private DasherData _dasherData;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _prepTime;

    private bool _prepFinished;
    public bool PrepFinished => _prepFinished;

    public DasherStatePrepare(DasherData dasherData, Rigidbody2D rb, SpriteRenderer spriteRenderer) {
        _rb = rb;
        _dasherData = dasherData;
        _spriteRenderer = spriteRenderer;
    }

    public void Tick() {
        _prepTime -= Time.deltaTime;
        if (_prepTime <= 0f) {
            _prepFinished = true;
        }
    }

    public void FixedTick() {
        _rb.velocity = Vector2.zero;
    }

    public void OnEnter() {
        _prepTime = _dasherData.prepTime;
        _spriteRenderer.color = Color.yellow;
    }

    public void OnExit() {
        _prepFinished = false;
        _spriteRenderer.color = Color.white;
    }
}
