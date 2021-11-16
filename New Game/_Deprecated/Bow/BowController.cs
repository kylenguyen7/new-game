using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using Unity.Mathematics;
using UnityEngine;

public class BowController : MonoBehaviour {
    [SerializeField] private Pullable _owner;
    [SerializeField] private GameObject _spearPrefab;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            FireHarpoon(KaleUtils.GetMousePosWorldCoordinates());
        }
    }

    private void FireHarpoon(Vector2 target) {
        var position = transform.position;
        var spear = Instantiate(_spearPrefab, position, quaternion.identity);
        var spearController = spear.GetComponent<SpearController>();
        var ropeController = spear.GetComponentInChildren<RopeController>();
        
        spearController.Init((target - (Vector2)position));
        ropeController.Init(transform, _owner);
    }
}
