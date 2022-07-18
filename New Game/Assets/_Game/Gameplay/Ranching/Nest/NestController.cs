using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NestController : WorldObjectController {
    [SerializeField] private Collider2D monsterPlacementBounds;
    private String _nestId;

    public String NestId {
        get => _nestId;
        set {
            _nestId = value;
            MonsterManager.Instance.RegisterNest(this);
        }
    }

    private void Awake() {
        NestId = Guid.NewGuid().ToString();
    }
    
    public override String GetMetaData() {
        return NestId;
    }
    
    public override void LoadMetaData(String data) {
        NestId = data;
    }

    public Vector2 GetRandomPositionInBounds() {
        var bounds = monsterPlacementBounds.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
}
