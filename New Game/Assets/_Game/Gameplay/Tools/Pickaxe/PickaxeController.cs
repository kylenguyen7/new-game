using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Common;

public class PickaxeController : ToolBase {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileOffset;
    [SerializeField] private GameObject bigSlashPrefab;
    [SerializeField] private float bigSlashOffset;

    [SerializeField] private float shakePower;
    [SerializeField] private float shakeDuration;

    [SerializeField] private PlayerController owner;
    [SerializeField] private float projectileCreationDelay;

    protected override bool InputTrigger => Input.GetMouseButton(0);

    protected override void Fire()
    {
        var mousePos = KaleUtils.GetMousePosWorldCoordinates();
        var toMouse = (mousePos - (Vector2)transform.position).normalized;

        String groupUuid = Guid.NewGuid().ToString();
        
        // // Create projectile
        // var projectile = Instantiate(projectilePrefab, (Vector2) transform.position + toMouse * projectileOffset,
        //         Quaternion.identity)
        //     .GetComponent<LinearProjectileController>();
        // projectile.Init(mousePos);

        StartCoroutine(CreateProjectile(projectileCreationDelay, toMouse, mousePos));
        
        
        // Associate projectiles to same projectile group
        // projectile.Uuid = groupUuid;
        // bigSlash.Uuid = groupUuid;
        
        CameraController.Instance.StartShake(shakeDuration, shakePower);
        
        owner.StartAttacking(toMouse);
    }

    private IEnumerator CreateProjectile(float delay, Vector2 direction, Vector2 mousePosition) {
        yield return new WaitForSeconds(delay);
        
        var bigSlash = Instantiate(bigSlashPrefab, transform, false).GetComponent<LinearProjectileController>();
        bigSlash.transform.localPosition = direction * bigSlashOffset;
        bigSlash.Init(mousePosition);
    }
}