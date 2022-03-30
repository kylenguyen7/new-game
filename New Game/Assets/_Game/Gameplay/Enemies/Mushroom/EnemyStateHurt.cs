
using UnityEngine;

/**
 * A generic "hurt" stunned state that sets a trigger called "hurt" on
 * the parent's animator for a set amount of time.
 */
public class EnemyHurtState : IState {
    private readonly EnemyBase _enemy;
    private readonly Animator _animator;
    
    private bool _done;
    public bool Done => _done;

    private static readonly float HURT_TIME = 0.5f;
    private float _timer;

    public EnemyHurtState(EnemyBase enemy, Animator animator) {
        _enemy = enemy;
        _animator = animator;
    }
    
    public void Tick() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) {
            _done = true;
        }
    }

    public void FixedTick() { }

    public void OnEnter() {
        _animator.SetTrigger("hurt");
        _done = false;
        _timer = HURT_TIME;
        _enemy.Velocity = Vector2.zero;
    }

    public void OnExit() { }
}