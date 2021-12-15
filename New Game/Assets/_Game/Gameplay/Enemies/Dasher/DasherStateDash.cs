using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherStateDash : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _dashTime;

    private bool _dashFinished;
    public bool DashFinished => _dashFinished;
    
    public DasherStateDash(DasherController dasherController, DasherData dasherData, Rigidbody2D rb, SpriteRenderer spriteRenderer) {
        _dasherController = dasherController;
        _dasherData = dasherData;
        _rb = rb;
        _spriteRenderer = spriteRenderer;
    }

    public void Tick() {
        if (_dashTime <= 0) {
            _dashFinished = true;
        }
        _dashTime -= Time.deltaTime;
    }

    public void FixedTick() {
        _rb.velocity = _dasherController.DashDir * _dasherData.dashSpeed;
    }

    public void OnEnter() {
        _dashTime = _dasherData.dashTime;
        _spriteRenderer.color = Color.red;
        _dasherController.gameObject.layer = 6;
        _dasherController.Collided = false;
    }

    public void OnExit() {
        _dashFinished = false;
        _rb.velocity = Vector2.zero;
        _spriteRenderer.color = Color.white;
        _dasherController.gameObject.layer = 0;
    }
}
