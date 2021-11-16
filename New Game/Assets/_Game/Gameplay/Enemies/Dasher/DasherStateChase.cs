using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherStateChase : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private Rigidbody2D _rb;

    private bool _chaseFinished;
    public bool ChaseFinished => _chaseFinished;

    public DasherStateChase(DasherController dasherController, DasherData dasherData, Rigidbody2D rb) {
        _dasherController = dasherController;
        _dasherData = dasherData;
        _rb = rb;
    }
    
    public void Tick() {
        if (Vector2.Distance(_dasherController.Target.position, _dasherController.transform.position) < _dasherData.chaseFinishedRadius) {
            _chaseFinished = true;
        }
        
        Debug.DrawLine(_dasherController.transform.position, _dasherController.Target.position);
    }

    public void FixedTick() {
        Vector2 toTarget = _dasherController.Target.position - _dasherController.transform.position;
        _rb.velocity = toTarget.normalized * _dasherData.chaseSpeed;
    }

    public void OnEnter() {
        _dasherController.SetTarget();
    }

    public void OnExit() {
        _dasherController.SetDashDir();
        _rb.velocity = Vector2.zero;
    }
}
