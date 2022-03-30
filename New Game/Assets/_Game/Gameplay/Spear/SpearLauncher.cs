using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpearLauncher : MonoBehaviour {
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private int maxSpears;
    [SerializeField] private GameObject sparksPrefab;
    [SerializeField] private float sparksSpread;
    [SerializeField] private float sparksCount;
    private int _currentSpears;

    private void Update() {
        if (Input.GetMouseButtonDown(1) && _currentSpears < maxSpears) {
            FireHarpoon(KaleUtils.GetMousePosWorldCoordinates());
        }
    }

    private void FireHarpoon(Vector2 target) {
        var position = transform.position;
        var direction = target - (Vector2)position;
        var spear = Instantiate(spearPrefab, position, Quaternion.identity);
        var spearController = spear.GetComponent<SpearController>();

        spearController.Init(direction, transform);

        _currentSpears += 1;
        spearController.OnDestroySpearCallback += () => _currentSpears -= 1;
        
        // Create effects
        for (int i = 0; i < sparksCount; i++) {
            var sparks = Instantiate(sparksPrefab, position, Quaternion.identity);
            sparks.transform.right = direction;
            sparks.transform.Rotate(0, 0, Random.Range(-sparksSpread, sparksSpread));
        }
    }
}