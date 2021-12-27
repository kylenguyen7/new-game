using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Rigidbody2D _followTarget;
    [Range(0f, 1f), SerializeField] private float _followSpeed;
    [Range(0f, 1f), SerializeField] private float _mouseTracking;
    [SerializeField] private Rigidbody2D _rb;

    private Vector2 _targetPos;

    private void FixedUpdate() {
        if (_followTarget == null) return;
        
        Vector2 mousePos = Input.mousePosition;
        
        // Range from [-0.5, 0.5] based on mouse position on screen. Clamp in case mouse is off-screen.
        float h = Mathf.Clamp((mousePos.x / Screen.width) - 0.5f, -0.5f, 0.5f);
        float v = Mathf.Clamp((mousePos.y / Screen.height) - 0.5f, -0.5f, 0.5f);

        float cameraHeightUnityUnits = 2 * Camera.main.orthographicSize;
        float cameraWidthUnityUnits = cameraHeightUnityUnits * Camera.main.aspect;
        Vector2 projectedFollowTargetPosition = _followTarget.position + _followTarget.velocity * Time.fixedDeltaTime;
        _targetPos = projectedFollowTargetPosition + new Vector2(h * cameraWidthUnityUnits, v * cameraHeightUnityUnits) * _mouseTracking;

        _rb.MovePosition((Vector2)transform.position + (_targetPos - (Vector2)transform.position) * _followSpeed);
    }
}