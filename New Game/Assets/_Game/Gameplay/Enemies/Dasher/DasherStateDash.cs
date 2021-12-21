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

    public void Tick() { }

    public void FixedTick() {
        if (_dashTime <= 0) {
            _rb.velocity = Vector2.zero;
            _dashFinished = true;
        } else {
            _rb.velocity = _dasherController.DashDir * _dasherData.dashSpeed;
        }
        _dashTime -= Time.deltaTime;
    }

    public void OnEnter() {
        _dashTime = _dasherData.dashTime;
        _spriteRenderer.color = Color.red;
        _dasherController.gameObject.layer = ApothecaryConstants.LAYER_DASHING;
        _dasherController.Collided = false;
    }

    public void OnExit() {
        _dashFinished = false;
        _spriteRenderer.color = Color.white;
        _dasherController.gameObject.layer = ApothecaryConstants.LAYER_ENEMIES;
    }
}
