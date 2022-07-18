using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MushroomController : EnemyBase {
    [SerializeField] private float speed;
    private bool _damaged;
    
    public float Speed => speed;


    public override EnemyType Type => EnemyType.MUSHROOM;

    public new void Awake() {
        base.Awake();

        var animator = GetComponentInChildren<Animator>();
        
        var chase = new MushroomStateChase(this);
        var hurt = new EnemyHurtState(this, animator);

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

    public override void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude, String uuid) {
        base.TakeDamage(damage, kbDirection, kbMagnitude, uuid);
        _damaged = true;
    }
}
