using UnityEngine;

public class EmptyRoomBrain : RoomBrain {
    [SerializeField] private GameObject emptyRoomStaticObjects;
    
    public override void PlaceStaticObjects() {
        Instantiate(emptyRoomStaticObjects, transform);
    }

    public override void Tick() {
        if (!_doorsOpen) {
            OpenDoors();
        }
    }
}