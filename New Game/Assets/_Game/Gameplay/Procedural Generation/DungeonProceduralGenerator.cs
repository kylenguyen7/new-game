using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonProceduralGenerator : MonoBehaviour {
    private static readonly float RANDOM_GIVE_UP_CHANCE = 0f;
    private static readonly int GRID_SIZE = 50;
    private bool[,] dungeon = new bool[50, 50];
    
    private Queue<Room> roomQueue = new Queue<Room>();
    [SerializeField] private int roomsToGenerate;
    private int _roomsGenerated;

    private void Start() {
        GenerateRooms();
        Debug.Log(dungeon);
    }

    private void GenerateRooms() {
        // Enqueue initial room
        roomQueue.Enqueue(new Room(GRID_SIZE / 2, GRID_SIZE / 2));
        _roomsGenerated++;
        
        while (roomQueue.Count > 0 && _roomsGenerated < roomsToGenerate) {
            Room prevRoom = roomQueue.Dequeue();
            foreach (Room neighbor in prevRoom.GetNeighbors()) {
                if (CanPlaceRoom(neighbor)) {
                    PlaceRoom(neighbor);
                    roomQueue.Enqueue(neighbor);
                    _roomsGenerated++;
                }

                if (_roomsGenerated == roomsToGenerate) break;
            }
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
    
    private void PlaceRoom(Room room) {
        int x = room.X;
        int y = room.Y;
        dungeon[x, y] = true;
    }

    private bool InBounds(int x, int y) {
        return x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE;
    }
}
