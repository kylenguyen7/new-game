using UnityEngine;
using _Common;

public class PickaxeController : ToolBase {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileOffset;
    [SerializeField] private GameObject bigSlashPrefab;
    [SerializeField] private float bigSlashOffset;

    protected override bool InputTrigger => Input.GetMouseButton(0);

    protected override void Fire() {
        var toMouse = KaleUtils.GetMousePosWorldCoordinates() - (Vector2) transform.position;

        // Create projectile
        var projectile = Instantiate(projectilePrefab, (Vector2) transform.position + toMouse * projectileOffset,
                Quaternion.identity)
            .GetComponent<LinearProjectile>();
        projectile.Init(toMouse);

        // Create big slash
        var bigSlash = Instantiate(bigSlashPrefab, (Vector2) transform.position + toMouse * bigSlashOffset,
            Quaternion.identity);
        bigSlash.transform.right = toMouse;
    }
}