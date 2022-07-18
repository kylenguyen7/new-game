using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Defines a common base class for all projectiles. Main features:
 * - List of tags that indicate which Damageables this should collide with.
 * - Parameters common to all projectiles: damage, knockback, and rb reference
 * - travelTime and travelTimer which are exposed to child classes
 *
 * Abstract classes
 * - Tick(): called each frame
 * - Init(Vector2 mousePos): called to set parameters like speed, direction, etc.
 */
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private List<String> damageableTagsList;
    private HashSet<String> damageableTagsSet;  // Set used for optimization
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float travelTime;
    [SerializeField] private bool destroyOnCollision = true;
    protected float travelTimer;
    public String Uuid { get; set; }

    private void Awake() {
        damageableTagsSet = new HashSet<String>();
        foreach(String damageableTag in damageableTagsList) {
            damageableTagsSet.Add(damageableTag);
        }
        Uuid = Guid.NewGuid().ToString();
    }

    protected abstract void Tick();

    public abstract void Init(Vector2 mousePosition);
    
    protected void Update() {
        if (travelTimer > travelTime) {
            Destroy(gameObject);
            return;
        }
        travelTimer += Time.deltaTime;
        Tick();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (damageableTagsSet.Contains(other.tag)) {
            var damageable = other.GetComponent<Damageable>();
            damageable.TakeDamage(damage, Vector2.right, knockback, Uuid);
            if(destroyOnCollision) Destroy(gameObject);
        }
        
        if (other.gameObject.layer == ApothecaryConstants.LAYER_TERRAIN) {
            if(destroyOnCollision) Destroy(gameObject);
        }
    }
}
