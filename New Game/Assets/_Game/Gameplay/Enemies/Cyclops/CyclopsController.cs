using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CyclopsController : EnemyBase {
    private Vector2 _movement;
    [SerializeField] private float _speed;

    private new void Update() {
        base.Update();
        _movement = new Vector2(
            (Input.GetKey(KeyCode.L) ? 1 : 0) - (Input.GetKey(KeyCode.J) ? 1 : 0),
            (Input.GetKey(KeyCode.I) ? 1 : 0) - (Input.GetKey(KeyCode.K) ? 1 : 0));
        _movement.Normalize();
    }

    private new void FixedUpdate() {
        base.FixedUpdate();
        Velocity = _movement * _speed;
    }
}
