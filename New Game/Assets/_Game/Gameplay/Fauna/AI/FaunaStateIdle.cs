
using UnityEngine;

public class FaunaStateIdle : FaunaState {
    private float _idleTime = 5f;
    private float _idleTimer;
    private bool _roaming;
    private Vector2 _roamingTarget;
    
    public FaunaStateIdle(RoamingFaunaController fauna) : base(fauna) { }
    
    public override void Tick() {
        if (_roaming) {
            _fauna.Rigidbody.velocity = (_roamingTarget - _fauna.Position).normalized * _fauna.RoamSpeed;

            if (Vector2.Distance(_fauna.Position, _roamingTarget) < 0.1f) {
                _roaming = false;
                _idleTimer = Random.Range(0f, _idleTime);
                _fauna.Rigidbody.velocity = Vector2.zero;
            }
        }
        else {
            if (_idleTimer <= 0) {
                _roaming = true;
                RandomizeRoamingTarget();
            }
            _idleTimer -= Time.deltaTime;
        }
    }

    public override void FixedTick() { }

    public override void OnEnter() {
        Target = null;
        _idleTimer = Random.Range(0f, _idleTime);
    }

    public override void OnExit() { }

    private void RandomizeRoamingTarget() {
        Vector2 position = _fauna.transform.position;
        _roamingTarget = position + 3 * Random.insideUnitCircle;
        _fauna.Facing = (_roamingTarget - position).normalized;
    }
}