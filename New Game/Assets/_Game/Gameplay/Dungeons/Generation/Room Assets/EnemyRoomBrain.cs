using System.Collections;
using UnityEngine;

public class EnemyRoomBrain : RoomBrain {
    [SerializeField] private GameObject enemyRoomStaticObjects;
    [SerializeField] private int minSpawnCount;
    [SerializeField] private int maxSpawnCount;
    [SerializeField] private GameObject dasherPrefab;
    [SerializeField] private GameObject shooterPrefab;
    [SerializeField] private GameObject mushroomPrefab;

    private int _enemyCount;
    private bool _enemiesSpawned;
    private Coroutine _spawnCoroutine;
    
    public override void PlaceStaticObjects() {
        Instantiate(enemyRoomStaticObjects, transform);
    }

    public override void Tick() {
        if (_doorsOpen) return;
        
        if (!_enemiesSpawned) {
            _spawnCoroutine = StartCoroutine(SpawnWaveCoroutine());
            _enemiesSpawned = true;
        }
        
        if (_spawnCoroutine == null && _enemyCount == 0) {
            OpenDoors();
        }
    }

    private IEnumerator SpawnWaveCoroutine() {
        yield return new WaitForSeconds(1f);
        int count = Random.Range(minSpawnCount, maxSpawnCount);
        for (int i = 0; i < count; i++) {
            var enemy = Random.value < 0.33f ? dasherPrefab : Random.value < 0.5f ? shooterPrefab : mushroomPrefab;
            var damageable = Instantiate(enemy, 
                    (Vector2)transform.position + new Vector2(Random.Range(-Bounds.x/2, Bounds.x/2), Random.Range(-Bounds.y/2, Bounds.y/2)),
                    Quaternion.identity)
                .GetComponent<Damageable>();
            _enemyCount += 1;
            damageable.OnDeathCallback += () => _enemyCount -= 1;
            yield return new WaitForSecondsRealtime(Random.Range(0.10f, 0.5f));
        }

        _spawnCoroutine = null;
    }
}