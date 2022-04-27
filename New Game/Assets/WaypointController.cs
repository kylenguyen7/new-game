using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaypointController : MonoBehaviour {
    [SerializeField] private List<GameObject> floraPrefabs;
    [SerializeField] private float spawnChance;

    private void Awake() {
        if (Random.value < spawnChance) {
            var flora = floraPrefabs[Random.Range(0, floraPrefabs.Count)];
            Instantiate(flora, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
