
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;


public class RoamingFaunaController : FaunaController {
    private StateMachine _stateMachine;
    [SerializeField] private Rigidbody2D rb;
    public float RoamSpeed;
    
    public Rigidbody2D Rigidbody => rb;
    public Vector2 Position => transform.position;

    private Transform _currentTarget;
    public Transform Target {
        get => _currentTarget;
        set {
            if (value == null) {
                rb.velocity = Vector2.zero;
            }
            _currentTarget = value;
        }
    }
    
    public Vector2 Facing {
        set => sprite.flipX = value.x > 0;
    }
    
    private void Awake() {
        _stateMachine = new StateMachine();
        
        var idle = new FaunaStateIdle(this);
        _stateMachine.Init(idle);

        transform.position = (Vector2)transform.position + 3 * Random.insideUnitCircle;
    }
    
    private void Update() {
        _stateMachine.Tick();
    }

    private void FixedUpdate() {
        if (Target != null) {
            rb.velocity = (Target.position - transform.position).normalized;
        }
        
        _stateMachine.FixedTick();
    }
}