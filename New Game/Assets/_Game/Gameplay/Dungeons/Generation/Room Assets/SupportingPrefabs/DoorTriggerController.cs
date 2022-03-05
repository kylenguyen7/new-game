using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerController : MonoBehaviour {
    [SerializeField] private float teleportDistance;
    [SerializeField] private Vector2 direction;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.position += (Vector3) direction.normalized * teleportDistance;

            
            int newX = DungeonProceduralGenerator.GetCurrentBrain().X + (int)direction.x;
            int newY = DungeonProceduralGenerator.GetCurrentBrain().Y + (int)direction.y;
            DungeonProceduralGenerator.SetCurrentBrain(newX, newY);
        }
    }
}