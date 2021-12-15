using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerController : MonoBehaviour {
    // Define boundaries somehow

    [SerializeField, Range(1, 10)]
    private int _minSpawnCount;
    [SerializeField, Range(1, 10)]
    private int _maxSpawnCount;
    [SerializeField] private GameObject _dasherPrefab;

    private int _enemyCount = 0;
    private Coroutine _spawnWaveCoroutine;

    private void Update() {
        if (_enemyCount == 0 && _spawnWaveCoroutine == null) {
            _spawnWaveCoroutine = StartCoroutine(SpawnWaveCoroutine());
        }
    }

    private IEnumerator SpawnWaveCoroutine() {
        yield return new WaitForSeconds(1f);
        int count = Random.Range(_minSpawnCount, _maxSpawnCount);
        for (int i = 0; i < count; i++) {
            var damageable = Instantiate(_dasherPrefab, new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)), Quaternion.identity)
                .GetComponent<Damageable>();
            damageable.OnDeathCallback += DecreaseCounter;
            _enemyCount += 1;
            yield return new WaitForSecondsRealtime(Random.Range(0.10f, 0.5f));
        }

        _spawnWaveCoroutine = null;
    }

    private void DecreaseCounter() {
        _enemyCount -= 1;
    }
}
