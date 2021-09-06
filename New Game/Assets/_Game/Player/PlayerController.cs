using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Player.PlayerStates;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private StateMachine _stateMachine = new StateMachine();

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BowController _bowController;
    [SerializeField] private Animator _playerSpriteAnimator;
    [SerializeField] private float speed;
    public float Speed {
        get => speed;
    }

    public Vector2 Facing { get; set; }

    private void Awake() {
        IState idle = new PlayerStateIdle(_rb, _bowController, _playerSpriteAnimator);
        IState moving = new PlayerStateMoving(this, _rb, _bowController, _playerSpriteAnimator);
        
        _stateMachine.AddTransition(idle, moving,
            () => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        _stateMachine.AddTransition(moving, idle,
            () => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0);
        _stateMachine.Init(idle);
    }

    private void Update() {
        _stateMachine.Tick();
    }

    private void FixedUpdate() {
        _stateMachine.FixedTick();
    }
}
