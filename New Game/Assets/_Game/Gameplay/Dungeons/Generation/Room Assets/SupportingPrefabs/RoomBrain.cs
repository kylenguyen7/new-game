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
    [SerializeField] private GameObject cavePrefab;
    [SerializeField] private List<GameObject> doors;
    [SerializeField] private List<GameObject> walls;
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
            doors[0].GetComponent<Animator>().SetTrigger("open");
            doors[0].GetComponent<Collider2D>().enabled = false;
        }
        if (DungeonProceduralGenerator.HasRoomAt(_gridX + 1, _gridY)) {
            doors[1].GetComponent<Animator>().SetTrigger("open");
            doors[1].GetComponent<Collider2D>().enabled = false;
        }
        if (DungeonProceduralGenerator.HasRoomAt(_gridX, _gridY - 1)) {
            doors[2].GetComponent<Animator>().SetTrigger("open");
            doors[2].GetComponent<Collider2D>().enabled = false;
        }
        if (DungeonProceduralGenerator.HasRoomAt(_gridX - 1, _gridY)) {
            doors[3].GetComponent<Animator>().SetTrigger("open");
            doors[3].GetComponent<Collider2D>().enabled = false;
        }

        if (DungeonProceduralGenerator.GetDungeonRoomType(_gridX, _gridY) == RoomType.END) {
            Instantiate(cavePrefab, transform.position, Quaternion.identity);
        }

        _doorsOpen = true;
    }

    public void Init(int gridX, int gridY) {
        _gridX = gridX;
        _gridY = gridY;

        if (!DungeonProceduralGenerator.HasRoomAt(_gridX, _gridY + 1)) {
            doors[0].SetActive(false);
        } else {
            walls[0].SetActive(false);
        }
        
        if (!DungeonProceduralGenerator.HasRoomAt(_gridX + 1, _gridY)) {
            doors[1].SetActive(false);
        } else {
            walls[1].SetActive(false);
        }

        if (!DungeonProceduralGenerator.HasRoomAt(_gridX, _gridY - 1)) {
            doors[2].SetActive(false);
        } else {
            walls[2].SetActive(false);
        }

        if (!DungeonProceduralGenerator.HasRoomAt(_gridX - 1, _gridY)) {
            doors[3].SetActive(false);
        } else {
            walls[3].SetActive(false);
        }
    }
}
