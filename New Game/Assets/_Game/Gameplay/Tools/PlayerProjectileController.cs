
using UnityEngine;

public class PlayerProjectileController : LinearProjectile {
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    
    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Destructible")) {
            var destructible = other.GetComponentInChildren<Destructible>();
            destructible.TakeDamage(damage, Vector2.zero, knockback);
            DestroyMe();
        }

        if (other.CompareTag("Enemy")) {
            var enemy = other.GetComponent<EnemyBase>();
            enemy.TakeDamage(damage, transform.right, knockback);
            DestroyMe();
        }
        
        if (other.gameObject.layer == ApothecaryConstants.LAYER_TERRAIN) {
            DestroyMe();
        }
    }
}