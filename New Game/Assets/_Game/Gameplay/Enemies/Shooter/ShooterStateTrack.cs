using System.Collections;
using UnityEngine;

public class ShooterStateTrack : IState {
    private ShooterController _shooter;
    private Rigidbody2D _rb;
    private Coroutine _attackCoroutine;
    private GameObject _shurikenPrefab;

    public ShooterStateTrack(ShooterController shooter, Rigidbody2D rb, GameObject shurikenPrefab) {
        _shooter = shooter;
        _rb = rb;
        _shurikenPrefab = shurikenPrefab;
    }

    private IEnumerator AttackCoroutine() {
        while (true) {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 3; i++) {
                Debug.Log($"spawning shuriken {i}");
                var shuriken =
                    GameObject.Instantiate(_shurikenPrefab, _shooter.transform.position, Quaternion.identity)
                        .GetComponent<ShurikenController>();
                shuriken.Init(GetDirectionToTarget());
                yield return new WaitForSeconds(0.75f);
            }
            yield return new WaitForSeconds(4f);
        }
    }

    private Vector2 GetDirectionToTarget() {
        return _shooter.Target.position - _shooter.transform.position;
    }
    
    public void Tick() {
        var transform = _shooter.transform;
        transform.right = GetDirectionToTarget();
    }

    public void FixedTick() {
        _shooter.Velocity = Vector2.zero;
    }

    public void OnEnter() {
        _attackCoroutine = _shooter.StartCoroutine(AttackCoroutine());
    }

    public void OnExit() {
        _shooter.StopCoroutine(_attackCoroutine);
    }
}