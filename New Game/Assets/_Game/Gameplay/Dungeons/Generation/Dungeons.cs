using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/**
 * Creates a 2D grid of RoomType representing a dungeon.
 * Follows immutable design pattern—if you want another dungeon,
 * create a new one.
 */
public static class Dungeons {

    // private static readonly float RANDOM_GIVE_UP_CHANCE = 0.33f;

    public static RoomType[,] GenerateDungeon(int size, int roomsToGenerate, float randomGiveUpChance) {
        // Initialize
        RoomType[,] dungeon = new RoomType[size, size];
        int roomsGenerated = 0;
        Queue<GridRoom> roomQueue = new Queue<GridRoom>();
        
        // Enqueue initial room
        GridRoom firstGridRoom = new GridRoom(size / 2, size / 2);
        PlaceRoom(dungeon, firstGridRoom);
        SetRoomType(dungeon, firstGridRoom, RoomType.START);
        roomQueue.Enqueue(firstGridRoom);
        roomsGenerated++;

        GridRoom prevGridRoom = null;
        while (roomQueue.Count > 0) {
            prevGridRoom = roomQueue.Dequeue();
            int neighborsGenerated = 0;
            foreach (GridRoom neighbor in prevGridRoom.GetNeighbors()) {
                if (roomsGenerated == roomsToGenerate) break;
                
                if (CanPlaceRoom(dungeon, neighbor, randomGiveUpChance)) {
                    PlaceRoom(dungeon, neighbor);
                    roomQueue.Enqueue(neighbor);
                    roomsGenerated++;
                    neighborsGenerated++;
                }
            }
            if (neighborsGenerated == 0) {
                SetRoomType(dungeon, prevGridRoom, RoomType.END);
            }
        }
        
        SetRoomType(dungeon, prevGridRoom, RoomType.BOSS);

        // Retry if not successful
        if (roomsGenerated < roomsToGenerate) {
            Debug.Log("Failed generation! Retrying :(");
            RoomType[,] newDungeon = GenerateDungeon(size, roomsToGenerate, randomGiveUpChance);
            return newDungeon;
        }
        return dungeon;
    }

    public static void PrintDungeon(RoomType[,] dungeon) {
        int width = dungeon.GetLength(0);
        int height = dungeon.GetLength(1);
        String dungeonString = "";
        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                dungeonString += (int)dungeon[x, y] + " ";
            }
            dungeonString += "\n";
        }
        Debug.Log(dungeonString);
    }

    private static bool CanPlaceRoom(RoomType[,] dungeon, GridRoom gridRoom, float randomGiveUpChance) {
        int x = gridRoom.X;
        int y = gridRoom.Y;
        
        // Position must be in bounds and not be occupied
        if (!InBounds(dungeon, x, y) || dungeon[x, y] != RoomType.EMPTY) return false;
        
        // Position cannot have more than one filled neighbor
        int filledNeighbors = 0;
        if (InBounds(dungeon, x - 1, y) && dungeon[x - 1, y] != RoomType.EMPTY) filledNeighbors++;
        if (InBounds(dungeon, x + 1, y) && dungeon[x + 1, y] != RoomType.EMPTY) filledNeighbors++;
        if (InBounds(dungeon, x, y - 1) && dungeon[x, y - 1] != RoomType.EMPTY) filledNeighbors++;
        if (InBounds(dungeon, x, y + 1) && dungeon[x, y + 1] != RoomType.EMPTY) filledNeighbors++;

        if (filledNeighbors > 1) return false;

        // Random give up
        if (Random.value < randomGiveUpChance) return false;
        
        return true;
    }
    
    private static void PlaceRoom(RoomType[,] dungeon, GridRoom gridRoom) {
        dungeon[gridRoom.X, gridRoom.Y] = RoomType.BASIC;
    }

    private static void SetRoomType(RoomType[,] dungeon, GridRoom gridRoom, RoomType type) {
        dungeon[gridRoom.X, gridRoom.Y] = type;
    }

    public static bool InBounds(RoomType[,] dungeon, int x, int y) {
        return x >= 0 && x < dungeon.GetLength(0) && y >= 0 && y < dungeon.GetLength(1);
    }

    public static bool InBounds(RoomBrain[,] dungeon, int x, int y) {
        return x >= 0 && x < dungeon.GetLength(0) && y >= 0 && y < dungeon.GetLength(1);
    }
}

public enum RoomType {
    EMPTY,
    NULL,       // Used for out-of-bounds checks
    BASIC,
    START,
    END,
    BOSS
}
