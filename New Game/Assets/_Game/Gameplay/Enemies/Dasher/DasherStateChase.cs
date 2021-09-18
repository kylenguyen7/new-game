using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherStateChase : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private Rigidbody2D _rb;
    private Transform _target;

    private bool _chaseFinished;
    public bool ChaseFinished => _chaseFinished;

    public DasherStateChase(DasherController dasherController, DasherData dasherData, Rigidbody2D rb) {
        _dasherController = dasherController;
        _dasherData = dasherData;
        _rb = rb;
    }
    
    public void Tick() {
        if (Vector2.Distance(_target.position, _dasherController.transform.position) < _dasherData.chaseFinishedRadius) {
            _chaseFinished = true;
        }
        
        Debug.DrawLine(_dasherController.transform.position, _target.position);
    }

    public void FixedTick() {
        Vector2 toTarget = _target.position - _dasherController.transform.position;
        _rb.velocity = toTarget.normalized * _dasherData.chaseSpeed;
    }

    public void OnEnter() {
        _target = _dasherController.GetClosestPlayer();
    }

    public void OnExit() {
        _rb.velocity = Vector2.zero;
    }
}
