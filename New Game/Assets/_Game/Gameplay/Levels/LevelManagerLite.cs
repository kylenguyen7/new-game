using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManagerLite : MonoBehaviour {
    public static LevelManagerLite Instance;
    
    [SerializeField] private GameObject _spawnerPrefab;
    [SerializeField] private List<Level> _levels;
    [SerializeField] private int _levelsPerPortal;
    
    private SpawnerController _spawner;
    private Level _level;
    private int _levelCount;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // If combat scene, start spawn sequence and reset counter
        if (scene.buildIndex == 0) {
            _level = _levels[Random.Range(0, _levels.Count)];
        
            // Create spawner and set spawn bounds
            _spawner = Instantiate(_spawnerPrefab, Vector3.zero, Quaternion.identity).GetComponent<SpawnerController>();
            _spawner.SetSpawnSize(_level.CameraBounds.x - 4, _level.CameraBounds.y - 4);
        
            // Create level
            Instantiate(_level.LevelPrefab, Vector3.zero, Quaternion.identity);
            
            // Set camera bounds
            CameraController.Instance.SetCameraSize(_level.CameraBounds);

            _levelCount += 1;
        } else if (scene.buildIndex == 1) {
            _levelCount = 0;
        }
        
        Debug.Log($"Scene loaded. Level count is {_levelCount}.");
    }

    private void Update() {
        // When spawner has finished last wave
        if (_spawner && _spawner.Finished && Input.GetKeyDown(KeyCode.LeftControl)) {
            if (_levelCount == _levelsPerPortal) {
                SceneManager.LoadScene(1);
            } else {
                SceneManager.LoadScene(0);
            }
        }
    }
}