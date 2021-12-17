using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Transform _followTarget;
    [Range(0f, 1f), SerializeField] private float _followSpeed;
    [Range(0f, 1f), SerializeField] private float _mouseTracking;

    private void Update() {
        if (_followTarget == null) return;
        
        Vector2 mousePos = Input.mousePosition;
        
        // Range from [-0.5, 0.5] based on mouse position on screen. Clamp in case mouse is off-screen.
        float h = Mathf.Clamp((mousePos.x / Screen.width) - 0.5f, -0.5f, 0.5f);
        float v = Mathf.Clamp((mousePos.y / Screen.height) - 0.5f, -0.5f, 0.5f);

        float cameraHeightUnityUnits = 2 * Camera.main.orthographicSize;
        float cameraWidthUnityUnits = cameraHeightUnityUnits * Camera.main.aspect;
        Vector3 targetPos = _followTarget.position + new Vector3(h * cameraWidthUnityUnits, v * cameraHeightUnityUnits, -10f) * _mouseTracking;

        transform.position = Vector3.Lerp(transform.position, targetPos, _followSpeed);
    }
}
