using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Player.PlayerStates;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private StateMachine _stateMachine = new StateMachine();

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _playerSpriteAnimator;
    [SerializeField] private float _speed;
    
    // Dashing
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;

    public float DashTime => _dashTime;
    public bool Dashing { get; set; }
    
    public float Speed => _speed;
    public float DashSpeed => _dashSpeed;

    public Vector2 Facing { get; set; }
    public Vector2 Heading { get; set; }

    private void Awake() {
        IState idle = new PlayerStateIdle(this, _rb, _playerSpriteAnimator);
        IState move = new PlayerStateMoving(this, _rb, _playerSpriteAnimator);
        IState dash = new PlayerStateDashing(this, _rb);
        
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

    private void Update() {
        _stateMachine.Tick();
    }

    private void FixedUpdate() {
        _stateMachine.FixedTick();
    }
}
