using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using _Game.Player.PlayerStates;
using UnityEngine;

public class PlayerController : ColorFlashDamageable {
    private StateMachine _stateMachine = new StateMachine();

    [SerializeField] public AudioSource _audioSource;
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
    // TODO: Change some of these to constants
    [SerializeField] public bool _canAttack;
    [SerializeField] public float _attackTime;                      // How long an attack lasts for
    [SerializeField] public float _finalAttackTime;                 // How long the final attack lasts for
    [SerializeField] public float _attackDisplacementSpeed;         // How fast the player dashes forward when attacking
    [SerializeField] public float _attackDisplacementDecel;         // How fast the dash decelerates
    [SerializeField] public float _attackOffset;                    // Distance to center of attack hitbox
    [SerializeField] public float _attackWidth;                     // Width of attack hitbox

    [SerializeField] public float _attackCooldown;
    [SerializeField] public float _attackComboResetTime;
    [SerializeField] public AudioClip _attackSfx;

    [HideInInspector] public float _attackCooldownTimer;
    
    // New attacking
    [SerializeField] public float attackTime;
    [SerializeField] public float attackMoveSpeed;
    public Vector2 AttackDirection { get; set; }
    
    // Working
    private Workable _currentlyWorking;
    private Workable _closestWorkable;
    public Workable ClosestWorkable => _closestWorkable;

    public bool Attacking { get; set; }

    // Cardinal direction (up, left, right, down)
    public Vector2 Facing { get; set; }
    // True direction
    public Vector2 Heading { get; set; }
    
    private bool Collided { get; set; }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube((Vector2)transform.position + Facing * _attackOffset, 
            new Vector2(_attackWidth, _attackWidth));
    }
    
    public void Teleport(Vector2 position, Vector2 direction) {
        transform.position = position;
        Facing = direction.normalized;
        Heading = direction.normalized;
        _playerSpriteAnimator.SetFloat("facingX", Facing.x);
        _playerSpriteAnimator.SetFloat("facingY", Facing.y);
    }

    private new void Awake() {
        base.Awake();
        
        IState idle = new PlayerStateIdle(this, _playerSpriteAnimator);
        IState move = new PlayerStateMoving(this, _playerSpriteAnimator);
        IState dash = new PlayerStateDashing(this, _playerSpriteAnimator);
        IState melee = new PlayerStateMeleeNew(this, _playerSpriteAnimator);
        
        _stateMachine.AddTransition(idle, move,
            () => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        _stateMachine.AddTransition(move, idle,
            () => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0);
        _stateMachine.AddAnyTransition(dash, 
            () => Input.GetKeyDown(KeyCode.LeftShift));
        _stateMachine.AddTransition(dash, idle,
            () => !Dashing);
        _stateMachine.AddTransition(dash, idle,
            () => {
                if (Collided) {
                    Collided = false;
                    return true;
                }
                return false;
            });
        _stateMachine.AddAnyTransition(melee, 
            () => Attacking);
        _stateMachine.AddTransition(melee, idle, 
            () => !Attacking);
        _stateMachine.Init(idle);
    }

    private new void Update() {
        base.Update();

        if (Time.timeScale == 0) return;
        
        if (_currentlyWorking == null) {
            _stateMachine.Tick();
        }
        
        // Set Collided to false if it wasn't culled by state machine's tick
        Collided = false;
        _attackCooldownTimer -= Time.deltaTime;
        _closestWorkable = GetClosestWorkableInRange();

        // Interact
        if (Input.GetKeyDown(KeyCode.E)) {
            Interact();
            Work();
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            StopWorking();
        }
    }

    private new void FixedUpdate() {
        base.FixedUpdate();
        
        if (_currentlyWorking == null) {
            _stateMachine.FixedTick();
        }
    }

    public override void TakeDamage(float damage, Vector2 kbDirection, float kbMagnitude, String uuid) {
        base.TakeDamage(damage, kbDirection, kbMagnitude, uuid);
        TimeStop._instance.StopTime();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Collided = true;
    }

    private void Interact() {
        var hits = Physics2D.RaycastAll(transform.position, Facing, 1f);
        
        foreach(var hit in hits) {
            var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            if (interactable != null) {
                interactable.Interact();
            }
        }
    }

    private void Work() {
        if (_closestWorkable == null) return;

        if (!_closestWorkable.CanWork()) {
            TextRise.Instance.CreateText(_closestWorkable.GetErrorMessage(), transform.position);
            return;
        }
        
        Velocity = Vector2.zero;
        _closestWorkable.StartWorking();
        _closestWorkable.OnWorkFinishCallback += StopWorking;
        _currentlyWorking = _closestWorkable;
        _playerSpriteAnimator.SetTrigger("foraging");
    }

    private void StopWorking() {
        if (_currentlyWorking != null) {
            _currentlyWorking.StopWorking();
            _currentlyWorking = null;
            _playerSpriteAnimator.SetTrigger("idle");
        }
    }

    /**
     * Initiates an attack state by setting a flag to be culled by state machine.
     * Separated like this to allow attacks to be initiated by separate controller
     * like a ToolController.
     */
    public void StartAttacking(Vector2 direction) {
        Attacking = true;
        AttackDirection = direction;
    }
    
    private Workable GetClosestWorkableInRange() {
        var hits = Physics2D.OverlapCircleAll(transform.position, 0.8f);

        Workable closestWorkable = null;
        float minDistance = 100f;
        foreach(var hit in hits) {
            var workable = hit.gameObject.GetComponent<Workable>();
            if (workable != null) {
                float distance = Vector2.Distance(transform.position, workable.transform.position);
                if (distance < minDistance) {
                    closestWorkable = workable;
                    minDistance = distance;
                }
            }
        }
        return closestWorkable;
    }
}
