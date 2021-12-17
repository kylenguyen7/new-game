using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterStateRoam : IState
{
    private ShooterController _shooter;
    private Rigidbody2D _rb;
    private float _leftX;
    private float _rightX;
    private float _botY;
    private float _topY;
    private float _waitTimer;
    private Vector2 _currentDestination;
    
    // Changeable variables
    private float _roamSpeed = 1f;
    private float _roamDist = 5f;
    private float _roamFinishedTolerance = 0.1f;
    private float _roamWaitTime = 2f;

    public ShooterStateRoam(ShooterController shooter, Rigidbody2D rb) {
        _shooter = shooter;
        _rb = rb;
        _leftX = _shooter.transform.position.x + _roamDist;
        _rightX = _shooter.transform.position.x - _roamDist;
        _botY = _shooter.transform.position.y + _roamDist;
        _topY = _shooter.transform.position.y - _roamDist;
        _waitTimer = _roamWaitTime;
    }

    private Vector2 PickRandomDestination() {
        return new Vector2(
            Random.Range(_leftX, _rightX),
            Random.Range(_botY, _topY)
        );
    }
    public void Tick() {
        if ((_currentDestination - (Vector2)_shooter.transform.position).magnitude < _roamFinishedTolerance) {
            if (_waitTimer < 0) {
                _currentDestination = PickRandomDestination();
                _waitTimer = _roamWaitTime;
            } else {
                _waitTimer -= Time.deltaTime;
            }
        }
    }

    public void FixedTick() {
        if ((_currentDestination - (Vector2) _shooter.transform.position).magnitude < _roamFinishedTolerance) return;
        
        var dir = (_currentDestination - (Vector2)_shooter.transform.position).normalized;
        _shooter.transform.right = dir;
        _rb.velocity = dir * _roamSpeed;
    }

    public void OnEnter() {
        _currentDestination = PickRandomDestination();
    }

    public void OnExit() { }
}
