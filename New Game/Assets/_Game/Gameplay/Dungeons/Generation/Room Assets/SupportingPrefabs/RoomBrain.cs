using System.Collections.Generic;
using UnityEngine;

/**
 * Controls a room, this component should be added to a room prefab.
 */
public abstract class RoomBrain : MonoBehaviour {
    private int _gridX;
    public int X => _gridX;
    private int _gridY;
    public int Y => _gridY;
    
    protected bool _doorsOpen = false;
    [SerializeField] private List<GameObject> doors = new List<GameObject>();
    [SerializeField] private Vector2 bounds;
    public Vector2 Bounds => bounds;
    
    public abstract void PlaceStaticObjects();
    public abstract void Tick();

    private void Awake() {
        PlaceStaticObjects();
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(bounds.x, bounds.y, 0));
    }

    protected void OpenDoors() {
        if (_doorsOpen) {
            Debug.LogError("Attempted to open the doors of a room that were already open.");
            return;
        }

        if (DungeonProceduralGenerator.HasRoomAt(_gridX, _gridY + 1)) {
            doors[0].SetActive(false);
        }
        if (DungeonProceduralGenerator.HasRoomAt(_gridX + 1, _gridY)) {
            doors[1].SetActive(false);
        }
        if (DungeonProceduralGenerator.HasRoomAt(_gridX, _gridY - 1)) {
            doors[2].SetActive(false);
        }
        if (DungeonProceduralGenerator.HasRoomAt(_gridX - 1, _gridY)) {
            doors[3].SetActive(false);
        }
    }

    public void Init(int gridX, int gridY) {
        _gridX = gridX;
        _gridY = gridY;
    }
}
