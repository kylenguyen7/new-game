using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DasherController : EnemyBase {
    [SerializeField] private DasherData _dasherData;
    [SerializeField] private float _damage;
    [SerializeField] private float _kbMagnitude;
    private Animator _animator;

    // These variables have to be in this function since they require a MonoBehaviour
    public Transform Target { get; private set; }
    public Vector2 DashDir { get; private set; }
    public bool Collided { get; set; }

    private new void Awake() {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
        
        var roam = new DasherStateRoam(this, _dasherData, _animator);
        var wait = new DasherStateWait(this, _dasherData);
        var chase = new DasherStateChase(this, _dasherData, _animator);
        var prep = new DasherStatePrepare(_dasherData, _rb, _spriteRenderer);
        var dash = new DasherStateDash(this, _dasherData, _spriteRenderer, _animator);
        var recover = new DasherStateRecover(_dasherData, _rb, _spriteRenderer);

        _stateMachine.AddTransition(roam, wait, () => roam.RoamFinished);
        _stateMachine.AddTransition(wait, roam, () => wait.WaitFinished);
        _stateMachine.AddTransition(roam, chase, () => GetDistanceToClosestPlayer() <= _dasherData.chaseStartRadius);
        _stateMachine.AddTransition(wait, chase, () => GetDistanceToClosestPlayer() <= _dasherData.chaseStartRadius);
        _stateMachine.AddTransition(chase, prep, () => GetDistanceToTarget() <= _dasherData.chaseFinishedRadius);
        _stateMachine.AddTransition(prep, dash, () => prep.PrepFinished);
        _stateMachine.AddTransition(dash, recover, () => dash.DashFinished);
        _stateMachine.AddTransition(recover, roam, () => recover.RecoverFinished);
        
        _stateMachine.AddTransition(dash, recover, () => {
            // Start recover state if collided with something last frame
            if (Collided) {
                Collided = false;
                return true;
            }
            return false;
        });
        
        _stateMachine.Init(roam);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _dasherData.chaseStartRadius);
        Gizmos.DrawWireSphere(transform.position, _dasherData.roamDestinationRadius);
        Gizmos.DrawWireSphere(transform.position, _dasherData.chaseFinishedRadius);
    }
    
    /**
     * Target and DashDir are maintained in DasherController since they are accessed
     * across multiple states.
     */
    public void SetTarget() {
        Target = GetClosestPlayer();
    }

    public void SetDashDir() {
        DashDir = (Target.position - transform.position).normalized;
    }

    private float GetDistanceToTarget() {
        return Vector2.Distance(Target.position, transform.position);
    }

    private float GetDistanceToClosestPlayer() {
        return Vector2.Distance(GetClosestPlayer().position, transform.position);
    }

    private void OnCollisionStay2D(Collision2D other) {
        // Deal damage only while dashing
        if (!(_stateMachine.getCurrentState() is DasherStateDash)) return;
        
        if (other.gameObject.CompareTag("Player")) {
            Vector2 dir = other.transform.position - transform.position;
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(_damage, dir, _kbMagnitude);
        }

        Collided = true;
    }

    public override void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude) {
        base.TakeDamage(damage, kbDirection, kbMagnitude);
        _animator.SetTrigger("hurt");
    }
}
