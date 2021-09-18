using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CyclopsController : EnemyBase {
    private Vector2 movement;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    private new void Awake() {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movement = new Vector2(
            (Input.GetKey(KeyCode.L) ? 1 : 0) - (Input.GetKey(KeyCode.J) ? 1 : 0),
            (Input.GetKey(KeyCode.I) ? 1 : 0) - (Input.GetKey(KeyCode.K) ? 1 : 0));
        movement.Normalize();
    }

    private void FixedUpdate() {
        _rb.velocity = movement * _speed;
    }
}
