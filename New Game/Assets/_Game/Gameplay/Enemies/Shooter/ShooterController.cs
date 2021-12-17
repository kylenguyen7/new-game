using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : EnemyBase {
    [SerializeField] private GameObject _shurikenPrefab;
    
    public Transform Target { get; private set; }
    
    private new void Awake() {
        base.Awake();
        Target = GetClosestPlayer();
        
        var track = new ShooterStateTrack(this, _rb, _shurikenPrefab);
        _stateMachine.Init(track);
        
    }
}
