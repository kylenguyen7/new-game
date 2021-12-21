using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherStateChase : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;

    private bool _chaseFinished;
    private Animator _animator;
    public bool ChaseFinished => _chaseFinished;

    public DasherStateChase(DasherController dasherController, DasherData dasherData, Animator animator) {
        _dasherController = dasherController;
        _dasherData = dasherData;
        _animator = animator;
    }
    
    public void Tick() {
        if (Vector2.Distance(_dasherController.Target.position, _dasherController.transform.position) < _dasherData.chaseFinishedRadius) {
            _chaseFinished = true;
        }
    }

    public void FixedTick() {
        Vector2 toTarget = _dasherController.Target.position - _dasherController.transform.position;
        _dasherController.Velocity = toTarget.normalized * _dasherData.chaseSpeed;
        _animator.SetFloat("facingX", Mathf.Sign(_dasherController.Velocity.x));
    }

    public void OnEnter() {
        _dasherController.SetTarget();
    }

    public void OnExit() {
        _dasherController.SetDashDir();
        _dasherController.Velocity = Vector2.zero;
    }
}
