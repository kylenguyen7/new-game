using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class SnSDamageable : Damageable {
    // Taking damage
    [SerializeField] protected Animator sNsAnimator;
    
    protected override IEnumerator DamageCoroutine() {
        sNsAnimator.SetTrigger("squash");
        _damageCoroutine = null;
        return null;
    }
}
