
using System.ServiceModel.Configuration;
using System.Windows.Forms.VisualStyles;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class LaunchProjectile : Projectile {
    [SerializeField] private float maxRange;

    // Parabola
    [SerializeField] private float maxHeight;
    [SerializeField] private GameObject launchedProjectile;
    private float a;

    public override void Init(Vector2 mousePosition) {
        // Outside of range
        Vector2 toMouse = (mousePosition - (Vector2) transform.position);
        if (toMouse.magnitude > maxRange) {
            mousePosition = (Vector2) transform.position + toMouse.normalized * maxRange;
        }

        float distance = (mousePosition - (Vector2) transform.position).magnitude;
        float speed = distance / travelTime;
        rb.velocity = toMouse.normalized * speed;

        a = -4 * maxHeight / Mathf.Pow(travelTime, 2);
    }

    protected override void Tick() {
        float projectileHeight = a * Mathf.Pow((travelTimer - travelTime / 2), 2) + maxHeight;
        launchedProjectile.transform.localPosition = new Vector2(0f, projectileHeight);
    }
}