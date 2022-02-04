using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room {
    public int X { get; }
    public int Y { get; }

    public Room(int x, int y) {
        X = x;
        Y = y;
    }

    public Room GetRandomNeighbor() {
        float value = Random.value;

        if (value < 0.25f) return new Room(X + 1, Y);
        if (value < 0.5f) return new Room(X - 1, Y);
        if (value < 0.75f) return new Room(X, Y + 1);
        return new Room(X, Y - 1);
    }

    public List<Room> GetNeighbors() {
        return new List<Room> {
            new Room(X + 1, Y),
            new Room(X - 1, Y),
            new Room(X, Y + 1),
            new Room(X, Y - 1),
        };
    }
}