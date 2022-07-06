
using UnityEngine;

public class BitletStateRoped : BitletState {
    public BitletStateRoped(BitletController bitlet) : base(bitlet) { }

    private float _distance;
    
    public override void Tick() {
        _distance -= Bitlet.RopedShortenSpeed;
        Bitlet.LineRenderer.SetPosition(0, Bitlet.transform.position);
        Bitlet.LineRenderer.SetPosition(1, Bitlet.RopeOrigin.position);
    }

    public override void FixedTick() {
        Vector3 targetPosition =
            Bitlet.RopeOrigin.position + (Bitlet.transform.position - Bitlet.RopeOrigin.position).normalized * _distance;
        Bitlet.Rigidbody.velocity = (targetPosition - Bitlet.transform.position) * Bitlet.RopedSpeed;
    }

    public override void OnEnter() {
        _distance = (Bitlet.RopeOrigin.position - Bitlet.transform.position).magnitude + 2f;
        
        Bitlet.LeftSweat.Play();
        Bitlet.RightSweat.Play();
        Bitlet.RopeSprite.enabled = true;
    }

    public override void OnExit() { }

    public float Distance => _distance;
}