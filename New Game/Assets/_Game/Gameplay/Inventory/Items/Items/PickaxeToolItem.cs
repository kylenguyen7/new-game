
using UnityEngine;

public class PickaxeToolItem : ToolItem {
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    
    public override void onAction(GameObject target) {
        var damageable = target.GetComponent<Damageable>();
        damageable.TakeDamage(damage, Vector2.right, knockback, null);
    }
}