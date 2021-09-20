using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherStateWait : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private float _waitTime;

    private bool _waitFinished;
    public bool WaitFinished => _waitFinished;

    public DasherStateWait(DasherController dasherController, DasherData dasherData) {
        _dasherController = dasherController;
        _dasherData = dasherData;
    }
    
    public void Tick() {
        _waitTime -= Time.deltaTime;
        if (_waitTime <= 0f) {
            _waitFinished = true;
        }
    }

    public void FixedTick() {
        _dasherController.Velocity = Vector2.zero;
    }

    public void OnEnter() {
        _waitTime = _dasherData.waitTime;
    }

    public void OnExit() {
        _waitFinished = false;
    }
}
