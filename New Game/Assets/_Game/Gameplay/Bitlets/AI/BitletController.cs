using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BitletController : MonoBehaviour {
    // CONSTANT VARS
    private StateMachine _stateMachine;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float roamSpeed;
    [SerializeField] private float roamTime;
    [SerializeField] private float ropedSpeed;
    [SerializeField] private float ropedShortenSpeed;
    [SerializeField] private SpriteRenderer ropeSprite;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem leftSweat;
    [SerializeField] private ParticleSystem rightSweat;
    
    private void OnDisable() {
        TooltipController.Instance.Title = "";
        TooltipController.Instance.Subtitle = "";
    }
    
    public float RoamSpeed => roamSpeed;
    public float RoamTime => roamTime;
    public float RopedSpeed => ropedSpeed;
    public float RopedShortenSpeed => ropedShortenSpeed;
    public Rigidbody2D Rigidbody => rb;
    
    public abstract Animator Animator { get; }

    public SpriteRenderer RopeSprite => ropeSprite;
    public LineRenderer LineRenderer => lineRenderer;
    public Transform RopeOrigin { get; private set; }
    public ParticleSystem LeftSweat => leftSweat;
    public ParticleSystem RightSweat => rightSweat;
    
    
    public bool Idling { get; set; }
    public bool Roaming { get; set; }
    
    private bool Roped { get; set; }

    private void Awake() {
        _stateMachine = new StateMachine();
        
        var idle = new BitletStateIdle(this);
        var roam = new BitletStateRoaming(this);
        var roped = new BitletStateRoped(this);
        
        _stateMachine.AddTransition(idle, roam, () => !Idling);
        _stateMachine.AddTransition(roam, idle, () => !Roaming);
        _stateMachine.AddAnyTransition(roped, () => {
            if (Roped) {
                return true;
            }

            return false;
        });
        _stateMachine.Init(idle);
    }
    
    private void Update() {
        _stateMachine.Tick();
    }

    private void FixedUpdate() {
        _stateMachine.FixedTick();
    }

    public void AttachRope(Transform origin) {
        RopeOrigin = origin; 
        Roped = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (Roped && other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}

public enum BitletType {
    HAPPY,
    SAD,
    HEADACHE
}