using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MushroomController : EnemyBase {
    [SerializeField] private float speed;
    [SerializeField] private float hurtTime;
    [SerializeField] private Animator animator;
    private bool _damaged;
    
    public float Speed => speed;
    public float HurtTime => hurtTime;


    public new void Awake() {
        base.Awake();
        var chase = new MushroomStateChase(this);
        var hurt = new MushroomStateHurt(this);

        _stateMachine.AddTransition(hurt, chase, () => hurt.Done);

        _stateMachine.AddAnyTransition(hurt, () => {
            // Start stunned state if took damage last frame
            if (_damaged) {
                _damaged = false;
                return true;
            }

            return false;
        });

        _stateMachine.Init(chase);
    }

    public override void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude) {
        base.TakeDamage(damage, kbDirection, kbMagnitude);
        _damaged = true;
        animator.SetTrigger("hurt");
    }
}
