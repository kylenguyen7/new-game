using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class LinearProjectile : Projectile {
    [SerializeField] private float _initialSpeed;
    
    public override void Init(Vector2 mousePosition)
    {
        var dir = (mousePosition - (Vector2)transform.position).normalized;
        transform.right = dir;
        rb.velocity = dir * _initialSpeed;
    }
}