﻿using System;
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

        if (!_attackQueued && _attackCount < COMBO_COUNT && _attackTimer <= 0.8f * _playerController._attackTime) {
            _attackQueued = Input.GetMouseButtonDown(0);
        }
    }

    public void FixedTick() {
        _attackTimer -= Time.deltaTime;
        _playerController.Velocity = Decelerate(_playerController.Velocity, _playerController._attackDisplacementDecel);
    }

    public void OnEnter() {
        _attackCount = 0;
        _playerController.Attacking = true;
        Attack();
    }

    private void Attack() {
        bool lastAttack = _attackCount == COMBO_COUNT;
        Vector2 toMouse = (KaleUtils.GetMousePosWorldCoordinates() - (Vector2)_playerController.transform.position).normalized;
        _playerController.Heading = toMouse;
        _playerController.Velocity = _playerController._attackDisplacementSpeed * toMouse;
        if (lastAttack) _playerController.Velocity *= 1.2f;

        if (Math.Abs(toMouse.x) > Math.Abs(toMouse.y)) {
            _playerController.Facing = new Vector2(Mathf.Sign(toMouse.x), 0);
        } else {
            _playerController.Facing = new Vector2(0, Mathf.Sign(toMouse.y));
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            (Vector2)_playerController.transform.position + _playerController.Facing * _playerController._attackOffset,
            new Vector2(_playerController._attackWidth, _playerController._attackWidth),
            0,
            1 << ApothecaryConstants.LAYER_ENEMIES);

        foreach (Collider2D hit in hits) {
            var enemy = hit.gameObject.GetComponent<EnemyBase>();
            enemy.TakeDamage(1f, toMouse, 1);
        }

        _animator.SetFloat("facingX", _playerController.Heading.x);
        _animator.SetFloat("facingY", _playerController.Heading.y);
        _animator.SetTrigger("attacking" + _attackCount % 2);
        _attackCount++;
        _attackTimer = lastAttack ? _playerController._lastAttackTime : _playerController._attackTime;
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

    public void OnExit() {
        _playerController._attackCooldownTimer = _playerController._attackCooldown;
    }
}