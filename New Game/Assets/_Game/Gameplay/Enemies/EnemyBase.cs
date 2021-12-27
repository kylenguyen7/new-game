using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : Damageable {
    protected StateMachine _stateMachine = new StateMachine();
    
    protected new void Update() {
        base.Update();
        _stateMachine.Tick();
    }
    
    private new void FixedUpdate() {
        base.FixedUpdate();
        _stateMachine.FixedTick();
    }
    
    protected Transform GetClosestPlayer() {
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
