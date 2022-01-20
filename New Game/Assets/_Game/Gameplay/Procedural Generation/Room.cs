using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Room : MonoBehaviour {
    private struct RoomBounds {
        private Vector2 _center;
        public Vector2 Center => _center;
        private Vector2 _size;
        public Vector2 Size => _size;

        public RoomBounds(Vector2 center, Vector2 size) {
            _center = center;
            _size = size;
        }
    }

    private Collider2D _collider;
    private RoomBounds _roomBounds;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
        var bounds = _collider.bounds;
        _roomBounds = new RoomBounds(bounds.center, bounds.size);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            CameraController.Instance.SetCameraBounds(_roomBounds.Center, _roomBounds.Size);
        }
    }
}
