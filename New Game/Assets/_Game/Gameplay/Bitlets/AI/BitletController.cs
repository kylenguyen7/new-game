using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitletController : MonoBehaviour {
    private StateMachine _stateMachine;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private float roamSpeed;
    [SerializeField] private float roamTime;
    
    public float RoamSpeed => roamSpeed;
    public float RoamTime => roamTime;
    public Rigidbody2D Rigidbody => rb;
    public Animator Animator => animator;
    public Vector2 Position => transform.position;
    
    public bool Idling { get; set; }
    public bool Roaming { get; set; }

    private void Awake() {
        _stateMachine = new StateMachine();
        
        var idle = new BitletStateIdle(this);
        var roam = new BitletStateRoaming(this);
        
        _stateMachine.AddTransition(idle, roam, () => !Idling);
        _stateMachine.AddTransition(roam, idle, () => !Roaming);
        _stateMachine.Init(idle);
    }
    
    private void Update() {
        _stateMachine.Tick();
    }

    private void FixedUpdate() {
        _stateMachine.FixedTick();
    }
}

public enum Ailment {
    NONE,
    HEADACHE,
    SAD
}

public class BitletInfo {
    private Ailment _ailment;
    public Ailment Ailment => _ailment;
}