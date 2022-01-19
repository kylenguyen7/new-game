using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Room : MonoBehaviour {
    private struct RoomBounds {
        private Vector2 _center;
        private Vector2 _extents;

        public RoomBounds(Vector2 center, Vector2 extents) {
            _center = center;
            _extents = extents;
        }
    }
    
    private Collider2D _collider;
    private RoomBounds _roomBounds;

    private void Awake() {
        _collider = GetComponent<BoxCollider2D>();
        var bounds = _collider.bounds;
        _roomBounds = new RoomBounds(bounds.center, bounds.extents);
    }
}
