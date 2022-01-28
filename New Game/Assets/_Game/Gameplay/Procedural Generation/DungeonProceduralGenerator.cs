using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonProceduralGenerator : MonoBehaviour {
    private static readonly int GRID_SIZE = 100;
    
    private bool[,] dungeon = new bool[GRID_SIZE, GRID_SIZE];
}
