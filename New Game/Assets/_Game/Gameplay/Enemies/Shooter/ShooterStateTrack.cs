using UnityEngine;

public class ShooterStateTrack : IState {
    private ShooterController _shooter;
    private Rigidbody2D _rb;
    
    public ShooterStateTrack(ShooterController shooter, Rigidbody2D rb) {
        _shooter = shooter;
        _rb = rb;
    }
    
    public void Tick() {
        var transform = _shooter.transform;
        transform.right = _shooter.Target.position - transform.position;
    }

    public void FixedTick() {
        _shooter.Velocity = Vector2.zero;
    }

    public void OnEnter() { }

    public void OnExit() { }
}