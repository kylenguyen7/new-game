using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridRoom {
    public int X { get; }
    public int Y { get; }

    public GridRoom(int x, int y) {
        X = x;
        Y = y;
    }

    public GridRoom GetRandomNeighbor() {
        float value = Random.value;

        if (value < 0.25f) return new GridRoom(X + 1, Y);
        if (value < 0.5f) return new GridRoom(X - 1, Y);
        if (value < 0.75f) return new GridRoom(X, Y + 1);
        return new GridRoom(X, Y - 1);
    }

    public List<GridRoom> GetNeighbors() {
        List<GridRoom> neighbors = new List<GridRoom> {
            new GridRoom(X + 1, Y),
            new GridRoom(X - 1, Y),
            new GridRoom(X, Y + 1),
            new GridRoom(X, Y - 1),
        };
        
        return neighbors;
    }

    public override string ToString() {
        return $"Room (x = {X}, y = {Y})";
    }
}