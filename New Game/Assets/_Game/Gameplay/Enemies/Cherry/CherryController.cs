using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CherryController : EnemyBase {
    [SerializeField] private float _chargeTime;
    [SerializeField] private float _cooldown;
    [SerializeField] private SpriteShake cherrySpriteShake;
    [SerializeField] private GameObject cherryStemPrefab;
    private bool _damaged;

    public float Cooldown => _cooldown;
    public float ChargeTime => _chargeTime;
    
    public override EnemyType Type { get; }
    
    public new void Awake() {
        base.Awake();

        var animator = GetComponentInChildren<Animator>();

        var hurt = new EnemyHurtState(this, animator);
        var active = new CherryStateActive(this, animator, cherrySpriteShake);
        
        _stateMachine.AddTransition(hurt, active, () => hurt.Done);
        _stateMachine.AddAnyTransition(hurt, () => {
            // Start stunned state if took damage last frame
            if (_damaged) {
                _damaged = false;
                return true;
            }

            return false;
        });
        _stateMachine.Init(active);
    }
    
    public override void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude) {
        base.TakeDamage(damage, kbDirection, kbMagnitude);
        _damaged = true;
    }

    public void Fire() {
        Instantiate(cherryStemPrefab, transform.position, Quaternion.identity);
    }
}
