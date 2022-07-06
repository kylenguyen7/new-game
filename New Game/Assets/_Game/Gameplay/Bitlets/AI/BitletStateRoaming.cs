
using UnityEngine;

public class BitletStateRoaming : BitletState {
    public BitletStateRoaming(BitletController bitlet) : base(bitlet) { }
    private float _timer;
    private Vector2 _direction;
    
    public override void Tick() {
        if (_timer <= 0) {
            Bitlet.Roaming = false;
        }
        _timer -= Time.deltaTime;
    }

    public override void FixedTick() {
        Bitlet.Rigidbody.velocity = _direction * Bitlet.RoamSpeed;
    }

    public override void OnEnter() {
        Bitlet.Roaming = true;
        if (Bitlet.Animator != null) {
            Bitlet.Animator.SetTrigger("roaming");
        }
        _timer = Random.Range(0.8f, 1.2f) * Bitlet.RoamTime;
        _direction = Random.insideUnitCircle.normalized;
    }

    public override void OnExit() {
        Bitlet.Roaming = false;
    }
}