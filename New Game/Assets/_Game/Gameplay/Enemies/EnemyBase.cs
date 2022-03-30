using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : Damageable {

    public abstract EnemyType Type { get; }
    protected StateMachine _stateMachine = new StateMachine();

    protected new void Awake() {
        base.Awake();
        
        // TODO: Consider using type-safe event system http://www.willrmiller.com/?p=87=
        // if managing deaths gets too tangled
        OnDeathCallback += () => BountyManager.Instance.ReceiveDeath(Type);
    }
    
    protected new void Update() {
        // TODO: remove
        if (Input.GetKeyDown(KeyCode.Q)) {
            Die();
        }
        base.Update();
        _stateMachine.Tick();
    }
    
    protected new void FixedUpdate() {
        base.FixedUpdate();
        _stateMachine.FixedTick();
    }
    
    public Transform GetClosestPlayer() {
        PlayerController closest = null;
        float minDist = float.MaxValue;
        foreach (var player in FindObjectsOfType<PlayerController>()) {
            float dist = Vector2.Distance(player.transform.position, transform.position);
            if (dist < minDist) {
                closest = player;
                minDist = dist;
            }
        }

        if (closest == null) {
            Debug.LogError("Enemy failed to find any players in scene.");
        }
        return closest.transform;
    }
}