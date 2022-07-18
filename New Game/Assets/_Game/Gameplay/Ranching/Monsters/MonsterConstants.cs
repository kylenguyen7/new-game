using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConstants : MonoBehaviour {
    public static MonsterConstants Instance;
    [SerializeField] private List<GameObject> allMonsterPrefabs;
    private Dictionary<String, GameObject> _speciesNameToMonsterPrefab = new Dictionary<string, GameObject>();
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        foreach(GameObject monster in allMonsterPrefabs) {
            _speciesNameToMonsterPrefab.Add(monster.name, monster);
        }
    }

    public static GameObject SpeciesNameToMonsterPrefab(String speciesName) {
        if (!Instance._speciesNameToMonsterPrefab.TryGetValue(speciesName, out var result)) {
            Debug.LogError($"MonsterConstants was unable to load item with speciesName {speciesName}!");
        }
        return result;
    }
}