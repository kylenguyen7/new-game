using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using Unity.Mathematics;
using UnityEngine;

public abstract class ToolBase : MonoBehaviour {
    [SerializeField] private Animator toolAnimator;
    [SerializeField] private float attackCooldown;
    
    private float attackTimer;

    protected delegate void OnAttackRecharged();

    protected OnAttackRecharged OnAttackRechargedCallback;

    protected abstract bool InputTrigger { get; }
    
    
    private void Update() {
        if (attackTimer < 0f) {
            if (InputTrigger) {
                Fire();
                toolAnimator.SetTrigger("hack");
                attackTimer = attackCooldown;
            }
        }
        else {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f) {
                OnAttackRechargedCallback?.Invoke();
            }
        }
    }

    protected abstract void Fire();
}
