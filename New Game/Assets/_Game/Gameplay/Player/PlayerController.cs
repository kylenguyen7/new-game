using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Player.PlayerStates;
using UnityEngine;

public class PlayerController : Damageable {
    private StateMachine _stateMachine = new StateMachine();
    
    [SerializeField] private Animator _playerSpriteAnimator;
    [SerializeField] private float _speed;
    
    // Dashing
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;

    public float DashTime => _dashTime;
    public bool Dashing { get; set; }
    
    public float Speed => _speed;
    public float DashSpeed => _dashSpeed;
    
    // Charging
    public float Flash;
    public float _currentFlash;
    private Material _material;

    // Cardinal direction (up, left, right, down)
    public Vector2 Facing { get; set; }
    // True direction
    public Vector2 Heading { get; set; }

    private new void Awake() {
        base.Awake();

        _material = _spriteRenderer.material;
        
        IState idle = new PlayerStateIdle(this, _playerSpriteAnimator);
        IState move = new PlayerStateMoving(this, _playerSpriteAnimator);
        IState charging = new PlayerStateCharging(this, _playerSpriteAnimator);
        IState dash = new PlayerStateDashing(this);
        
        _stateMachine.AddTransition(idle, move,
            () => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        _stateMachine.AddTransition(move, idle,
            () => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0);
        _stateMachine.AddTransition(idle, dash,
            () => Input.GetKeyDown(KeyCode.LeftShift));
        _stateMachine.AddTransition(move, dash,
            () => Input.GetKeyDown(KeyCode.LeftShift));
        _stateMachine.AddTransition(dash, idle,
            () => !Dashing);

        _stateMachine.AddAnyTransition(charging, () => Input.GetMouseButtonDown(1));
        _stateMachine.AddTransition(charging, idle, () => Input.GetMouseButtonUp(1));
        _stateMachine.Init(idle);
    }

    private new void Update() {
        base.Update();
        _stateMachine.Tick();
        
        _currentFlash = Mathf.Lerp(_currentFlash, Flash, 0.05f);
        _material.SetFloat("_Flash", _currentFlash);
    }

    private new void FixedUpdate() {
        base.FixedUpdate();
        _stateMachine.FixedTick();
    }

    public override void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude) {
        base.TakeDamage(damage, kbDirection, kbMagnitude);
        TimeStop._instance.StopTime();
    }
}
