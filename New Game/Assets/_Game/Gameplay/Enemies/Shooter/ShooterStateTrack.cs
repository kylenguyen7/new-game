using System.Collections;
using UnityEngine;

public class ShooterStateTrack : IState {
    private ShooterController _shooter;
    private Coroutine _attackCoroutine;
    private GameObject _shurikenPrefab;
    private SpriteRenderer _spriteRenderer;

    public ShooterStateTrack(ShooterController shooter, SpriteRenderer spriteRenderer, GameObject shurikenPrefab) {
        _shooter = shooter;
        _shurikenPrefab = shurikenPrefab;
        _spriteRenderer = spriteRenderer;
    }

    private IEnumerator AttackCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            _spriteRenderer.color = Color.red;
            for (int i = 0; i < 4; i++) {
                yield return new WaitForSeconds(0.5f);
                var shuriken =
                    GameObject.Instantiate(_shurikenPrefab, _shooter.transform.position, Quaternion.identity)
                        .GetComponent<Projectile>();
                shuriken.Init(_shooter.Target.position);
            }
            yield return new WaitForSeconds(0.25f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(3f);
        }
    }

    public void Tick() {
        // var transform = _shooter.transform;
        // transform.right = _shooter.GetDirectionToTarget();
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