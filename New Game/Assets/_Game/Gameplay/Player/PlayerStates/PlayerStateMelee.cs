using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using _Game.Player.PlayerStates;
using Unity.Mathematics;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerStateMelee : IState {
    private static int COMBO_COUNT = 3;
    private PlayerController _playerController;
    private Animator _animator;

    private float _attackTimer;
    private int _comboCounter; // The number of attacks that have occurred
    private bool _attackQueued;
    private float _prevAttackTime;

    public PlayerStateMelee(PlayerController playerController, Animator animator) {
        _playerController = playerController;
        _animator = animator;
    }

    public void Tick() {
        // Only allow attacks up to the COMBO_COUNT
        bool attacksRemaining = _comboCounter < COMBO_COUNT;
        if (!_attackQueued && attacksRemaining) {
            _attackQueued = Input.GetMouseButtonDown(0);
        }
    }

    public void FixedTick() {
        if (_attackTimer <= 0) {
            if (_attackQueued) {
                Attack();
                _attackQueued = false;
            } else {
                // It is impossible to queue another attack once this has been set, since
                // input is checked in Tick() but before Tick() can be called, the state transition
                // out of Attack will trigger.
                _playerController.Attacking = false;
            }
        }
        _attackTimer -= Time.deltaTime;
        _playerController.Velocity = Decelerate(_playerController.Velocity, _playerController._attackDisplacementDecel);
    }

    public void OnEnter() {
        float endOfPrevAttack = _prevAttackTime + _playerController._attackTime;
        if (Time.time - endOfPrevAttack >= _playerController._attackComboResetTime) {
            _comboCounter = 0;
        }
        _attackQueued = true;
        _playerController.Attacking = true;
        
        Debug.Log($"Melee started {Time.time}");
    }

    private void Attack() { 
        Vector2 toMouse = (KaleUtils.GetMousePosWorldCoordinates() - (Vector2)_playerController.transform.position).normalized;
        SetDirection(toMouse);
        DealDamage(toMouse);
        
        _playerController._audioSource.pitch = 1 + _comboCounter * 0.1f;
        _playerController._audioSource.PlayOneShot(_playerController._attackSfx);
        _attackTimer = _comboCounter == COMBO_COUNT - 1 ? _playerController._finalAttackTime : _playerController._attackTime;
        _prevAttackTime = Time.time;
        _comboCounter++;
    }
    
    public void OnExit() {
        if (_comboCounter == COMBO_COUNT) {
            _playerController._attackCooldownTimer = _playerController._attackCooldown;
            _comboCounter = 0;
        }
        
        Debug.Log($"Melee ended {Time.time}");
    }
    
    private void SetDirection(Vector2 dir) {
        _playerController.Heading = dir;
        _playerController.Velocity = _playerController._attackDisplacementSpeed * dir;

        if (Math.Abs(dir.x) > Math.Abs(dir.y)) {
            _playerController.Facing = new Vector2(Mathf.Sign(dir.x), 0);
        } else {
            _playerController.Facing = new Vector2(0, Mathf.Sign(dir.y));
        }
        
        _animator.SetFloat("facingX", _playerController.Heading.x);
        _animator.SetFloat("facingY", _playerController.Heading.y);
        _animator.SetTrigger("attacking" + _comboCounter % 3);
    }

    private void DealDamage(Vector2 dir) {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            (Vector2)_playerController.transform.position + _playerController.Facing * _playerController._attackOffset,
            new Vector2(_playerController._attackWidth, _playerController._attackWidth),
            0,
            1 << ApothecaryConstants.LAYER_ENEMIES);

        foreach (Collider2D hit in hits) {
            var enemy = hit.gameObject.GetComponent<EnemyBase>();
            enemy.TakeDamage(1f, dir, 0);
        }
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
}