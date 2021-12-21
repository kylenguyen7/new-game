using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DasherStateDash : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private float _dashTime;
    private bool _dashFinished;
    public bool DashFinished => _dashFinished;
    
    public DasherStateDash(DasherController dasherController, DasherData dasherData, SpriteRenderer spriteRenderer, Animator animator) {
        _dasherController = dasherController;
        _dasherData = dasherData;
        _spriteRenderer = spriteRenderer;
        _animator = animator;
    }

    public void Tick() {}

    public void FixedTick() {
        if (_dashTime <= 0) {
            _dasherController.Velocity = Vector2.zero;
            _dashFinished = true;
        } else {
            _dasherController.Velocity = _dasherController.DashDir * _dasherData.dashSpeed;
        }
        
        _animator.SetFloat("facingX", Mathf.Sign(_dasherController.Velocity.x));
        _dashTime -= Time.deltaTime;
    }

    public void OnEnter() {
        _dashTime = _dasherData.dashTime;
        _spriteRenderer.color = Color.red;
        _dasherController.gameObject.layer = ApothecaryConstants.LAYER_DASHING;
        _dasherController.Collided = false;
        _animator.SetTrigger("dash");
    }

    public void OnExit() {
        _dashFinished = false;
        _spriteRenderer.color = Color.white;
        _dasherController.gameObject.layer = ApothecaryConstants.LAYER_ENEMIES;
        _animator.SetTrigger("idle");
    }
}
