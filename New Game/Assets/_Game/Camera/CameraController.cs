using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Rigidbody2D _followTarget;
    [Range(0f, 1f), SerializeField] private float _followSpeed;
    [Range(0f, 1f), SerializeField] private float _mouseTracking;
    private Rigidbody2D _rb;

    private Vector2 _center = Vector2.zero;
    // TODO: remove SerializeField
    [SerializeField] private Vector2 _size;

    private float _cameraWidthUnityUnits;
    private float _cameraHeightUnityUnits;

    public static CameraController Instance;
    
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

    private void FixedUpdate() {
        if (_followTarget == null) return;
        
        Vector2 mousePos = Input.mousePosition;
        
        // Range from [-0.5, 0.5] based on mouse position on screen. Clamp in case mouse is off-screen.
        float h = Mathf.Clamp((mousePos.x / Screen.width) - 0.5f, -0.5f, 0.5f);
        float v = Mathf.Clamp((mousePos.y / Screen.height) - 0.5f, -0.5f, 0.5f);
        
        float minX = _center.x - _size.x / 2 + _cameraWidthUnityUnits / 2;
        float maxX = _center.x + _size.x / 2 - _cameraWidthUnityUnits / 2;
        float minY = _center.y - _size.y / 2 + _cameraHeightUnityUnits / 2;
        float maxY = _center.y + _size.y / 2 - _cameraHeightUnityUnits / 2;
        Vector2 projectedFollowTargetPosition = _followTarget.position + _followTarget.velocity * Time.fixedDeltaTime;
        Vector2 targetPos = projectedFollowTargetPosition + new Vector2(h * _cameraWidthUnityUnits, v * _cameraHeightUnityUnits) * _mouseTracking;

        targetPos.x = minX < maxX ? Mathf.Clamp(targetPos.x, minX, maxX) : _center.x;
        targetPos.y = minY < maxY ? Mathf.Clamp(targetPos.y, minY, maxY) : _center.y;
        
        _rb.MovePosition((Vector2)transform.position + (targetPos - (Vector2)transform.position) * _followSpeed);
    }

    public void SetCameraSize(Vector2 size) {
        _size = size;
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_center, _size);
    }
}