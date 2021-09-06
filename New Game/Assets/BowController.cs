using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using Unity.Mathematics;
using UnityEngine;

public class BowController : MonoBehaviour {
    [SerializeField] private GameObject _spearPrefab;
    private List<SpearLineConnectionController> _connections = new List<SpearLineConnectionController>();

    public Vector2 Pull { get; set; }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            FireHarpoon(KaleUtils.GetMousePosWorldCoordinates());
        }
        
        Vector2 pull = Vector2.zero;
        foreach (var connection in _connections) {
            pull += connection.Pull;
        }
        Pull = pull;
    }

    private void FireHarpoon(Vector2 target) {
        var position = transform.position;
        var spear = Instantiate(_spearPrefab, position, quaternion.identity);
        var spearController = spear.GetComponent<SpearController>();
        var spearLineConnectionController = spear.GetComponentInChildren<SpearLineConnectionController>();
        
        spearController.Init((target - (Vector2)position));
        spearLineConnectionController.Init(transform);
        
        _connections.Add(spearLineConnectionController);
    }
}
