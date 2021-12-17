using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : EnemyBase {
    [SerializeField] private GameObject _shurikenPrefab;
    [SerializeField] private float _trackRadius;

    private Vector2 _gizmoCenter;
    
    public Transform Target { get; private set; }
    
    private new void Awake() {
        base.Awake();
        Target = GetClosestPlayer();

        _gizmoCenter = transform.position;
        
        var track = new ShooterStateTrack(this, _shurikenPrefab);
        var roam = new ShooterStateRoam(this, _rb);
        
        // _stateMachine.AddTransition(roam, track, () => GetDistanceToTarget() <= _trackRadius);
        // _stateMachine.AddTransition(track, roam, () => GetDistanceToTarget() > _trackRadius);
        _stateMachine.Init(track);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(_gizmoCenter, new Vector3(10f, 10f, 0f));
        Gizmos.DrawWireSphere(transform.position, _trackRadius);
    }

    public Vector2 GetDirectionToTarget() {
        return GetVectorToTarget().normalized;
    }

    private float GetDistanceToTarget() {
        return GetVectorToTarget().magnitude;
    }

    private Vector2 GetVectorToTarget() {
        return Target.position - transform.position;
    }
}
