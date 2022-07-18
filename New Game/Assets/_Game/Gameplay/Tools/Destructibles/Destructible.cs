
using System;
using System.Collections;
using UnityEngine;

public class Destructible : Damageable {
    [SerializeField] private GameObject hitSparkPrefab;
    
    // Taking damage
    [SerializeField] protected Animator sNsAnimator;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakePower;

    private void OnEnable() {
        OnDeathCallback += BigShake;
    }

    private void OnDisable() {
        OnDeathCallback -= BigShake;
    }

    private void Shake() {
        CameraController.Instance.StartShake(shakeDuration, shakePower);
    }

    private void BigShake() {
        CameraController.Instance.StartShake(shakeDuration * 2, shakePower * 2);
    }

    protected override IEnumerator DamageCoroutine()
    {
        Shake();
        sNsAnimator.SetTrigger("squash");
        Instantiate(hitSparkPrefab, transform.position, Quaternion.identity);
        _damageCoroutine = null;
        return null;
    }
}