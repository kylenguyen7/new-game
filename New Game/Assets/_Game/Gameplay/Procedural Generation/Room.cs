using System;
using System.Collections.Generic;
using UnityEngine;

public class Room {
    /*
    private enum Direction {
        UP,
        LEFT,
        RIGHT,
        DOWN
    }
    */

    private List<Point> body;
    // private Dictionary<Direction, List<Point>> walls = new Dictionary<Direction, List<Point>>();

    public Room(List<Point> body) {
        this.body = new List<Point>(body);
    }
    
    private bool Place(bool[,] board, int x, int y) {
        // Verify room can be placed
        foreach (Point p in body) {
            int pieceX = x + p.x;
            int pieceY = y + p.y;
            if (pieceX < 0 || pieceX >= board.GetLength(0) || pieceY < 0 || pieceY >= board.GetLength(1)) return false;
            if (board[pieceX, pieceY]) return false;
        }
        
        // Place room
        foreach (Point p in body) {
            int pieceX = x + p.x;
            int pieceY = y + p.y;
            board[pieceX, pieceY] = true;
        }
        return true;
    }
}

public struct Point {
    public int x;
    public int y;

    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
}