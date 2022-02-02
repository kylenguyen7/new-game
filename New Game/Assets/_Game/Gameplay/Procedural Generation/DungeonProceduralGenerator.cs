using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonProceduralGenerator : MonoBehaviour {
    private static readonly float RANDOM_GIVE_UP_CHANCE = 0f;
    private static readonly int GRID_SIZE = 50;
    private bool[,] dungeon = new bool[50, 50];
    
    private Queue<Room> roomQueue = new Queue<Room>();
    [SerializeField] private int roomsToGenerate = Random.Range(8, 10);
    private int _roomsGenerated = 0;

    private void GenerateRooms() {
        // Enqueue initial room
        roomQueue.Enqueue(new Room(GRID_SIZE / 2, GRID_SIZE / 2));
        _roomsGenerated++;
        
        while (roomQueue.Count > 0) {
            Room prevRoom = roomQueue.Dequeue();
            // TODO: choose random neighbor of room
        }
    }

    private bool CanPlaceRoom(Room room) {
        int x = room.X;
        int y = room.Y;
        
        // Position must be in bounds and not be occupied
        if (!InBounds(x, y) || dungeon[x, y]) return false;
        
        // Position cannot have a filled neighbor
        if (InBounds(x - 1, y) && dungeon[x - 1, y]) return false;
        if (InBounds(x + 1, y) && dungeon[x + 1, y]) return false;
        if (InBounds(x, y - 1) && dungeon[x, y - 1]) return false;
        if (InBounds(x, y + 1) && dungeon[x, y + 1]) return false;

        // Random give up
        if (Random.value < RANDOM_GIVE_UP_CHANCE) return false;
        
        return true;
    }
    
    private void PlaceRoom(int x, int y) {
        dungeon[x, y] = true;
        
    }

    private bool InBounds(int x, int y) {
        return x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE;
    }
}
