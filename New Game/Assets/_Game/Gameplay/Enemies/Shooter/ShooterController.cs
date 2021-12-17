using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : EnemyBase {
    public Transform Target { get; private set; }
    
    private new void Awake() {
        base.Awake();
        Target = GetClosestPlayer();
        
        var track = new ShooterStateTrack(this, _rb);
        _stateMachine.Init(track);
        
    }
}
