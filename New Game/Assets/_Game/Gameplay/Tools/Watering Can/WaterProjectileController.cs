
using System;
using UnityEngine;

public class WaterProjectileController : LaunchProjectile {
    [SerializeField] private GameObject waterSplashEffect;
    
    private void OnDestroy() {
        Instantiate(waterSplashEffect, transform.position, Quaternion.identity);
    }
}