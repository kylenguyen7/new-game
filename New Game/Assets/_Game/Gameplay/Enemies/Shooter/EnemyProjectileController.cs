
using UnityEngine;

public class EnemyProjectileController : LinearProjectile {
    [SerializeField] private float _damage;

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(_damage, Vector2.zero, 0);
            DestroyMe();
        }

        if (other.gameObject.layer == ApothecaryConstants.LAYER_TERRAIN) {
            DestroyMe();
        }
    }
}
