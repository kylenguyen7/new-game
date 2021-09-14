using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using Unity.Mathematics;
using UnityEngine;

public class BowController2 : MonoBehaviour {
    [SerializeField] private GameObject _spearPrefab;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            FireHarpoon(KaleUtils.GetMousePosWorldCoordinates());
        }
    }

    private void FireHarpoon(Vector2 target) {
        var position = transform.position;
        var spear = Instantiate(_spearPrefab, position, Quaternion.identity);
        var spearController = spear.GetComponent<SpearController2>();

        spearController.Init((target - (Vector2)position), transform);
    }
}
