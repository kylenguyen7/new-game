
using System.ServiceModel.Configuration;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class LaunchProjectile : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    // [SerializeField] private float speed;
    [SerializeField] private float maxRange;
    [SerializeField] private GameObject launchedProjectile;
    
    // Parabola
    [SerializeField] private float maxHeight;
    private float a;
    
    [SerializeField] private float travelTime;
    private float travelTimer;
    
    public void Init(Vector2 target) {
        // Outside of range
        Vector2 toTarget = (target - (Vector2) transform.position);
        if (toTarget.magnitude > maxRange) {
            target = (Vector2) transform.position + toTarget.normalized * maxRange;
        }

        float distance = (target - (Vector2) transform.position).magnitude;
        float speed = distance / travelTime;
        rb.velocity = toTarget.normalized * speed;

        a = -4 * maxHeight / Mathf.Pow(travelTime, 2);
    }

    private void Update() {
        if (travelTimer > travelTime) {
            OnReachDestination();
            Destroy(gameObject);
        }
        travelTimer += Time.deltaTime;

        float projectileHeight = a * Mathf.Pow((travelTimer - travelTime / 2), 2) + maxHeight;
        launchedProjectile.transform.localPosition = new Vector2(0f, projectileHeight);
    }

    protected abstract void OnReachDestination();
}