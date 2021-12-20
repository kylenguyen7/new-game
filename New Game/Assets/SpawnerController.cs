using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerController : MonoBehaviour {
    [SerializeField] private float _spawnWidth;
    [SerializeField] private float _spawnHeight;

    [SerializeField, Range(1, 10)]
    private int _minSpawnCount;
    [SerializeField, Range(1, 10)]
    private int _maxSpawnCount;
    [SerializeField] private GameObject _dasherPrefab;
    [SerializeField] private GameObject _shooterPrefab;
    
    private int _enemyCount = 0;
    private Coroutine _spawnWaveCoroutine;

    private void Update() {
        if (_enemyCount == 0 && _spawnWaveCoroutine == null) {
            _spawnWaveCoroutine = StartCoroutine(SpawnWaveCoroutine());
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_spawnWidth, _spawnHeight, 0f));
    }

    private IEnumerator SpawnWaveCoroutine() {
        yield return new WaitForSeconds(1f);
        int count = Random.Range(_minSpawnCount, _maxSpawnCount);
        for (int i = 0; i < count; i++) {
            var enemy = Random.value < 0.5f ? _dasherPrefab : _shooterPrefab;
            var damageable = Instantiate(enemy, new Vector2(Random.Range(-_spawnWidth/2, _spawnWidth/2), Random.Range(-_spawnHeight/2, _spawnHeight/2)), Quaternion.identity)
                .GetComponent<Damageable>();
            damageable.OnDeathCallback += DecreaseCounter;
            damageable.gameObject.layer = LayerMask.NameToLayer("Enemies");
            Debug.Log(damageable.gameObject);
            Debug.Log(damageable.gameObject.layer);
            _enemyCount += 1;
            yield return new WaitForSecondsRealtime(Random.Range(0.10f, 0.5f));
        }

        _spawnWaveCoroutine = null;
    }

    private void DecreaseCounter() {
        _enemyCount -= 1;
    }
}
