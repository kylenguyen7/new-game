
using UnityEngine;

public class WaterProjectileController : LaunchProjectile {
    [SerializeField] private GameObject waterSplashEffect;
    
    protected override void OnReachDestination() {
        Instantiate(waterSplashEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}