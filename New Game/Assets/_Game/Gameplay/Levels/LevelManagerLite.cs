using System.Collections.Generic;
using UnityEngine;
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
        _spawner.SetSpawnSize(_level.CameraBounds.x - 4, _level.CameraBounds.y - 4);
        
        // Create level
        Instantiate(_level.LevelPrefab, Vector3.zero, Quaternion.identity);
    }

    private void Start() {
        // Set camera bounds
        CameraController.Instance.SetCameraSize(_level.CameraBounds);
    }

    private void Update() {
        // When spawner has finished last wave
        if (_spawner.Finished && Input.GetKeyDown(KeyCode.LeftControl)) {
            SceneManager.LoadScene(0);
        }
    }
}