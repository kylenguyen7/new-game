using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerStateDashing : IState {
    private PlayerController _playerController;
    private Animator _animator;
    private Vector2 _dashDir;
    private float _dashTime;
    
    public PlayerStateDashing(PlayerController playerController, Animator animator) {
        _playerController = playerController;
        _animator = animator;
    }

    public void Tick() { }

    public void FixedTick() {
        if (_dashTime >= _playerController.DashTime) {
            _playerController.Dashing = false;
            // Set velocity to 0 here, since another FixedUpdate might occur before the 
            // an Update loop where the stateMachine can call OnExit to set velocity to 0.
            _playerController.Velocity = Vector2.zero;
            _playerController.gameObject.layer = ApothecaryConstants.LAYER_PLAYER;
        }
        _dashTime += Time.deltaTime;
    }

    public void OnEnter() {
        _playerController.Dashing = true;
        _playerController.gameObject.layer = ApothecaryConstants.LAYER_DASHING;
        
        _dashDir = _playerController.Velocity.magnitude == 0 ? _playerController.Facing : _playerController.Heading;
        // _dashDir = (KaleUtils.GetMousePosWorldCoordinates() - (Vector2)_playerController.transform.position).normalized;
        
        _playerController.Velocity = _dashDir * _playerController.DashSpeed;
        
        _dashTime = 0f;
        _animator.SetTrigger("idle");
        _animator.SetFloat("facingX", _dashDir.x);
        _animator.SetFloat("facingY", _dashDir.y);
    }

    public void OnExit() { }
}
