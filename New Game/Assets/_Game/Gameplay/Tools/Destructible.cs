
using System.Collections;
using UnityEngine;

public class Destructible : Damageable {
    [SerializeField] private GameObject hitSparkPrefab;
    
    // Taking damage
    [SerializeField] protected Animator sNsAnimator;
    
    protected override IEnumerator DamageCoroutine() {
        sNsAnimator.SetTrigger("squash");
        Instantiate(hitSparkPrefab, transform.position, Quaternion.identity);
        _damageCoroutine = null;
        return null;
    }
}