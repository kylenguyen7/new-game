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
    [SerializeField] private float _dashTime;                   // Better to end dash from time or distance?
    [SerializeField] private float _dashDistance;

    public float DashTime => _dashTime;
    public float DashDistance => _dashDistance;
    public bool Dashing { get; set; }
    
    public float Speed => _speed;
    public float DashSpeed => _dashSpeed;
    
    // Attacking
    // Some of these can become constants later
    [SerializeField] private float _attackTime;                     // How long an attack lasts for
    [SerializeField] public float _attackDisplacementSpeed;         // How fast the player dashes forward when attacking
    [SerializeField] public float _attackDisplacementDecel;         // How fast the dash decelerates
    [SerializeField] public float _attackOffset;                    // Distance to center of attack hitbox
    [SerializeField] public float _attackWidth;                     // Width of attack hitbox
    [SerializeField] public float _attackCooldown;

    [HideInInspector] public float _attackCooldownTimer;
    
    public bool Attacking { get; set; }
    public float AttackTime => _attackTime;

    // Cardinal direction (up, left, right, down)
    public Vector2 Facing { get; set; }
    // True direction
    public Vector2 Heading { get; set; }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube((Vector2)transform.position + Facing * _attackOffset, 
            new Vector2(_attackWidth, _attackWidth));
    }

    private new void Awake() {
        base.Awake();
        
        IState idle = new PlayerStateIdle(this, _playerSpriteAnimator);
        IState move = new PlayerStateMoving(this, _playerSpriteAnimator);
        IState dash = new PlayerStateDashing(this, _playerSpriteAnimator);
        IState melee = new PlayerStateMelee(this, _playerSpriteAnimator);
        
        _stateMachine.AddTransition(idle, move,
            () => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        _stateMachine.AddTransition(move, idle,
            () => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0);
        _stateMachine.AddAnyTransition(dash, 
            () => Input.GetKeyDown(KeyCode.Space));
        _stateMachine.AddTransition(dash, idle,
            () => !Dashing);
        _stateMachine.AddAnyTransition(melee, 
            () => Input.GetMouseButtonDown(0) && _attackCooldownTimer < 0);
        _stateMachine.AddTransition(melee, idle, 
            () => !Attacking);
        
        _stateMachine.Init(idle);
    }

    private new void Update() {
        base.Update();
        _stateMachine.Tick();
        
        _attackCooldownTimer -= Time.deltaTime;
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
