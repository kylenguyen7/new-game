using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DasherStatePrepare : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private SpriteRenderer _spriteRenderer;

    private float _prepTime;
    private bool _prepFinished;
    public bool PrepFinished => _prepFinished;

    public DasherStatePrepare(DasherController dasherController, DasherData dasherData, SpriteRenderer spriteRenderer) {
        _dasherController = dasherController;
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
        _dasherController.Velocity = Vector2.zero;
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
