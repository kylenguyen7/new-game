using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDashing : IState {
    private PlayerController _playerController;
    private Vector2 _dashDir;
    private float _dashTime;
    
    public PlayerStateDashing(PlayerController playerController) {
        _playerController = playerController;
    }

    public void Tick() {
        _dashTime += Time.deltaTime;
        if (_dashTime >= _playerController.DashTime) {
            _playerController.Dashing = false;
        }
    }

    public void FixedTick() {
        _playerController.Velocity = _dashDir * _playerController.DashSpeed;
    }

    public void OnEnter() {
        _playerController.Dashing = true;
        
        _dashDir = _playerController.Velocity.magnitude == 0 ? _playerController.Facing : _playerController.Heading;
        _dashTime = 0f;
    }

    public void OnExit() {
        _playerController.Velocity = Vector2.zero;
    }
}
