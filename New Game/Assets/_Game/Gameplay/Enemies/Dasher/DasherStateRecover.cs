using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DasherStateRecover : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private SpriteRenderer _spriteRenderer;
    private float _recoverTime;

    private bool _recoverFinished;
    public bool RecoverFinished => _recoverFinished;

    public DasherStateRecover(DasherController dasherController, DasherData dasherData, SpriteRenderer spriteRenderer) {
        _dasherController = dasherController;
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
        _dasherController.Velocity = Vector2.zero;
    }

    public void OnEnter() {
        _recoverFinished = false;
        _recoverTime = _dasherData.recoverTime;
        _spriteRenderer.color = Color.gray;
    }

    public void OnExit() {
        _spriteRenderer.color = Color.white;
    }
}
