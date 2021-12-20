using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using _Game.Player.PlayerStates;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerStateMelee : IState {
    private PlayerController _playerController;
    private Animator _animator;
    private float _attackTimer = 0f;
    private int _attackCount = 0;
    private bool _attackQueued;
    
    public PlayerStateMelee(PlayerController playerController, Animator animator) {
        _playerController = playerController;
        _animator = animator;
    }
    
    public void Tick() {
        if (_attackTimer <= 0) {
            if (_attackQueued) {
                Attack();
                _attackQueued = false;
            }
            else {
                _playerController.Attacking = false;
            }
        }

        if (!_attackQueued && _attackTimer <= 0.8f * _playerController.AttackTime) {
            _attackQueued = Input.GetMouseButtonDown(0);
        }
        _attackTimer -= Time.deltaTime;
    }

    public void FixedTick() {
        _playerController.Velocity = Decelerate(_playerController.Velocity, _playerController._attackDisplacementDecel);
    }

    public void OnEnter() {
        _playerController.Attacking = true;
        Attack();
    }

    private void Attack() {
        Vector2 toMouse = (KaleUtils.GetMousePosWorldCoordinates() - (Vector2)_playerController.transform.position).normalized;
        _playerController.Facing = toMouse;
        _playerController.Velocity = _playerController._attackDisplacementSpeed * toMouse;
        
        _animator.SetFloat("facingX", _playerController.Facing.x);
        _animator.SetFloat("facingY", _playerController.Facing.y);
        _animator.SetTrigger("attacking" + _attackCount);
        _attackCount = (_attackCount + 1) % 2;
        _attackTimer = _playerController.AttackTime;
    }

    private Vector2 Decelerate(Vector2 velocity, float decel) {
        float x = velocity.x;
        float y = velocity.y;

        if (x > 0) {
            x = Mathf.Max(0, x - decel);
        }
        else {
            x = Mathf.Min(0, x + decel);
        }
        
        if (y > 0) {
            y = Mathf.Max(0, y - decel);
        }
        else {
            y = Mathf.Min(0, y + decel);
        }

        return new Vector2(x, y);
    }

    public void OnExit() { }
}