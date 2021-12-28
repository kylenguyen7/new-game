using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManagerLite : MonoBehaviour {
    [SerializeField] private GameObject _spawnerPrefab;
    [SerializeField] private List<Level> _levels;

    private Level _level;
    private SpawnerController _spawner;

    private void Awake() {
        _level = _levels[Random.Range(0, _levels.Count)];
        
        // Create spawner and set spawn bounds
        _spawner = Instantiate(_spawnerPrefab, Vector3.zero, Quaternion.identity).GetComponent<SpawnerController>();
        _spawner.SetSpawnSize(_level._cameraBounds.x - 4, _level._cameraBounds.y - 4);
        
        // Create level
        Instantiate(_level._levelPrefab, Vector3.zero, Quaternion.identity);
    }

    private void Start() {
        // Set camera bounds
        CameraController.Instance.SetCameraSize(_level._cameraBounds);
    }

    private void Update() {
        // When spawner has finished last wave
        if (_spawner.Finished && Input.GetKeyDown(KeyCode.LeftControl)) {
            SceneManager.LoadScene(0);
        }
    }
}