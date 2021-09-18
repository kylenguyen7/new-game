using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherStateWait : IState {
    private DasherData _dasherData;
    private Rigidbody2D _rb;
    private float _waitTime;

    private bool _waitFinished;
    public bool WaitFinished => _waitFinished;

    public DasherStateWait(DasherData dasherData, Rigidbody2D rb) {
        _rb = rb;
        _dasherData = dasherData;
    }
    
    public void Tick() {
        _waitTime -= Time.deltaTime;
        if (_waitTime <= 0f) {
            _waitFinished = true;
        }
    }

    public void FixedTick() {
        _rb.velocity = Vector2.zero;
    }

    public void OnEnter() {
        _waitTime = _dasherData.waitTime;
    }

    public void OnExit() {
        _waitFinished = false;
    }
}
