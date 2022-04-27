using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EmptyRoomBrain : RoomBrain {
    [SerializeField] private List<GameObject> possibleStaticObjects;
    
    public override void PlaceStaticObjects() {
        var staticObjects = possibleStaticObjects[Random.Range(0, possibleStaticObjects.Count)];
        Instantiate(staticObjects, transform);
    }

    public override void Tick() {
        if (!_doorsOpen) {
            OpenDoors();
        }
    }
}