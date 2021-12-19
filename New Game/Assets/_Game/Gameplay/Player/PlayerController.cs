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

    // Cardinal direction (up, left, right, down)
    public Vector2 Facing { get; set; }
    // True direction
    public Vector2 Heading { get; set; }

    private new void Awake() {
        base.Awake();
        
        IState idle = new PlayerStateIdle(this, _playerSpriteAnimator);
        IState move = new PlayerStateMoving(this, _playerSpriteAnimator);
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
            
        _stateMachine.Init(idle);
    }

    private new void Update() {
        base.Update();
        _stateMachine.Tick();
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
