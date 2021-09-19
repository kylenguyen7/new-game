using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DasherStateRecover : IState {
    private DasherData _dasherData;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _recoverTime;

    private bool _recoverFinished;
    public bool RecoverFinished => _recoverFinished;

    public DasherStateRecover(DasherData dasherData, Rigidbody2D rb, SpriteRenderer spriteRenderer) {
        _rb = rb;
        _dasherData = dasherData;
        _spriteRenderer = spriteRenderer;
    }

    public void Tick() {
        _recoverTime -= Time.deltaTime;
        if (_recoverTime <= 0f) {
            _recoverFinished = true;
        }
    }

    public void FixedTick() {
        _rb.velocity = Vector2.zero;
    }

    public void OnEnter() {
        _recoverTime = _dasherData.recoverTime;
        _spriteRenderer.color = Color.gray;
    }

    public void OnExit() {
        _recoverFinished = false;
        _spriteRenderer.color = Color.white;
    }
}
