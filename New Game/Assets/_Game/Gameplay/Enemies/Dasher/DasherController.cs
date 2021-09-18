using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DasherController : EnemyBase {
    private StateMachine _stateMachine = new StateMachine();
    private Rigidbody2D _rb;
    [SerializeField] private DasherData _dasherData;

    private new void Awake() {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        
        var roam = new DasherStateRoam(this, _dasherData, _rb);
        var wait = new DasherStateWait(_dasherData, _rb);
        var chase = new DasherStateChase(this, _dasherData, _rb);
        
        _stateMachine.AddTransition(roam, wait, () => roam.RoamFinished);
        _stateMachine.AddTransition(wait, roam, () => wait.WaitFinished);
        _stateMachine.AddTransition(roam, chase, () => GetDistanceToClosestPlayer() <= _dasherData.chaseStartRadius);
        _stateMachine.AddTransition(wait, chase, () => GetDistanceToClosestPlayer() <= _dasherData.chaseStartRadius);
        _stateMachine.Init(roam);
    }

    private void Update() {
        _stateMachine.Tick();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _dasherData.chaseStartRadius);
        Gizmos.DrawWireSphere(transform.position, _dasherData.roamDestinationRadius);
    }

    private void FixedUpdate() {
        _stateMachine.FixedTick();
    }

    private float GetDistanceToClosestPlayer() {
        return Vector2.Distance(GetClosestPlayer().position, transform.position);
    }

    public Transform GetClosestPlayer() {
        PlayerController closest = null;
        float minDist = float.MaxValue;
        foreach (var player in FindObjectsOfType<PlayerController>()) {
            float dist = Vector2.Distance(player.transform.position, transform.position);
            if (dist < minDist) {
                closest = player;
                minDist = dist;
            }
        }

        if (closest == null) {
            Debug.LogError("Dasher failed to find any players in scene.");
        }
        return closest.transform;
    }
}
