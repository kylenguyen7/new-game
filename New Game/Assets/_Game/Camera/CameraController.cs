using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour {
    [SerializeField] private Rigidbody2D followTarget;
    [Range(0f, 1f), SerializeField] private float followSpeed;
    [Range(0f, 1f), SerializeField] private float mouseTracking;
    [SerializeField] private float maxSpeed;
    private Rigidbody2D _rb;

    [SerializeField] private Vector2 _center = Vector2.zero;
    // TODO: remove SerializeField
    [SerializeField] private Vector2 bounds;
    [SerializeField] private bool inCombatRoom;     // Uses room bounds if in combat room

    private float _cameraWidthUnityUnits;
    private float _cameraHeightUnityUnits;

    public static CameraController Instance;
    
    // Shake
    private float shakeTimeRemaining, shakePower, shakeFadeRate;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _cameraWidthUnityUnits = 2 * Camera.main.orthographicSize * Camera.main.aspect;
        _cameraHeightUnityUnits = 2 * Camera.main.orthographicSize;
    }

    private void Update() {
        if (inCombatRoom) {
            _center = DungeonProceduralGenerator.GetCurrentBrain().gameObject.transform.position;
            bounds = DungeonProceduralGenerator.GetCurrentBrain().Bounds;
        }
    }

    public void StartShake(float duration, float magnitude) {
        shakeTimeRemaining = duration;
        shakePower = magnitude;

        shakeFadeRate = shakePower / shakeTimeRemaining;
    }

    private void FixedUpdate() {
        if (followTarget == null) return;
        
        Vector2 mousePos = Input.mousePosition;
        
        // Range from [-0.5, 0.5] based on mouse position on screen. Clamp in case mouse is off-screen.
        float h = Mathf.Clamp((mousePos.x / Screen.width) - 0.5f, -0.5f, 0.5f);
        float v = Mathf.Clamp((mousePos.y / Screen.height) - 0.5f, -0.5f, 0.5f);
        
        
        Vector2 projectedFollowTargetPosition = followTarget.position + followTarget.velocity * Time.fixedDeltaTime;
        Vector2 targetPos = projectedFollowTargetPosition + new Vector2(h * _cameraWidthUnityUnits, v * _cameraHeightUnityUnits) * mouseTracking;
        Vector2 clampedTargetPos = ClampToBounds(targetPos);
        
        // Flip the hotbar at the bottom of a level
        if (clampedTargetPos.y > targetPos.y && Math.Abs(clampedTargetPos.y - _center.y) > 0.01f) {
            HotbarController.Instance.SetPosition(HotbarController.HotbarPosition.TOP);
        } else {
            HotbarController.Instance.SetPosition(HotbarController.HotbarPosition.BOTTOM);
        }
        
        Vector2 currentPosition = transform.position;
        Vector2 newPos;
        if (((clampedTargetPos - currentPosition) * followSpeed).magnitude > maxSpeed) {
            newPos = currentPosition + (clampedTargetPos - currentPosition).normalized * maxSpeed;
        } else {
            newPos = Vector2.Lerp(currentPosition, clampedTargetPos, followSpeed);
        }
        
        
        
        
        if (shakeTimeRemaining > 0) {
            float xAmount = shakePower * Random.Range(-1f, 1f);
            float yAmount = shakePower * Random.Range(-1f, 1f);

            newPos.x += xAmount;
            newPos.y += yAmount;

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeRate * Time.deltaTime);
        }
        
        _rb.MovePosition(new Vector3(newPos.x, newPos.y, -10));
        //
        // // Weird bug where velocity accumulates due to shaking
        // _rb.velocity = Vector2.zero;
    }

    /**
     * Immediately sets the camera position to a certain position within the bounds.
     * Useful when transitioning scenes.
     */
    public void SetPositionWithinBounds(Vector2 targetPos) {
        targetPos = ClampToBounds(targetPos);
        transform.position = new Vector3(targetPos.x, targetPos.y, -10);
    }

    private Vector2 ClampToBounds(Vector2 position) {
        float minX = _center.x - bounds.x / 2 + _cameraWidthUnityUnits / 2;
        float maxX = _center.x + bounds.x / 2 - _cameraWidthUnityUnits / 2;
        float minY = _center.y - bounds.y / 2 + _cameraHeightUnityUnits / 2;
        float maxY = _center.y + bounds.y / 2 - _cameraHeightUnityUnits / 2;
        
        float x = minX < maxX ? Mathf.Clamp(position.x, minX, maxX) : _center.x;
        float y = minY < maxY ? Mathf.Clamp(position.y, minY, maxY) : _center.y;

        return new Vector2(x, y);
    }

    public void SetCameraBounds(Vector2 center, Vector2 bounds) {
        _center = center;
        this.bounds = bounds;
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_center, bounds);
    }
}