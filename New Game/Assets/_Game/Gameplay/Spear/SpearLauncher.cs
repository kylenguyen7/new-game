using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using Unity.Mathematics;
using UnityEngine;

public class SpearLauncher : MonoBehaviour {
    [SerializeField] private GameObject _spearPrefab;
    [SerializeField] private int _maxSpears;
    private int _currentSpears;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _currentSpears < _maxSpears) {
            FireHarpoon(KaleUtils.GetMousePosWorldCoordinates());
        }
    }

    private void FireHarpoon(Vector2 target) {
        var position = transform.position;
        var spear = Instantiate(_spearPrefab, position, Quaternion.identity);
        var spearController = spear.GetComponent<SpearController>();

        spearController.Init((target - (Vector2)position), transform);

        _currentSpears += 1;
        spearController.OnDestroySpearCallback += () => _currentSpears -= 1;
    }
}