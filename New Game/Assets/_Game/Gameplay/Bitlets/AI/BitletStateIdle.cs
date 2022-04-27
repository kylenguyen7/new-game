
using UnityEngine;

public class BitletStateIdle : BitletState { 
    private float _timer;

    public BitletStateIdle(BitletController bitlet) : base(bitlet) { }

    public override void Tick() {
        if (_timer <= 0) {
            Bitlet.Idling = false;
        }
        _timer -= Time.deltaTime;
    }

    public override void FixedTick() {
        Bitlet.Rigidbody.velocity = Vector2.zero;
    }

    public override void OnEnter() {
        Bitlet.Animator.SetTrigger("idle");
        Bitlet.Idling = true;
        _timer = Random.Range(3f, 5f);
    }

    public override void OnExit() {
        Bitlet.Idling = false;
    }
}