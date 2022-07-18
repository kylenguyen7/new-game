

using UnityEngine;

public class PlayerStateMeleeNew : IState {
    private PlayerController _playerController;
    private Animator _animator;
    private float _attackTimeRemaining;
    
    public PlayerStateMeleeNew(PlayerController playerController, Animator animator) {
        _playerController = playerController;
        _animator = animator;
    }
    
    public void Tick() {
        if (_attackTimeRemaining <= 0) {
            _playerController.Attacking = false;
        }
        _attackTimeRemaining -= Time.deltaTime;
    }

    public void FixedTick() { }

    public void OnEnter() {
        _animator.SetFloat("facingX", _playerController.AttackDirection.x);
        _animator.SetFloat("facingY", _playerController.AttackDirection.y);
        _animator.SetTrigger("attacking");
        
        _playerController.Velocity = _playerController.AttackDirection * _playerController.attackMoveSpeed;
        _attackTimeRemaining = _playerController.attackTime;
    }

    public void OnExit() { }
}