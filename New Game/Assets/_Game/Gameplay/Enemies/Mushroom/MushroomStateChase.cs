
using UnityEngine;

public class MushroomStateChase : IState {
    private MushroomController _mushroom;
    
    public MushroomStateChase(MushroomController mushroom) {
        _mushroom = mushroom;
    }

    public void Tick() {
        // Hacky "face the player"
        Vector2 direction = _mushroom.GetClosestPlayer().position - _mushroom.transform.position;
        float xScale = 0.75f;
        if (direction.x < 0) {
            xScale = -xScale;
        }
        
        var transform = _mushroom.transform;
        var scale = transform.localScale;
        scale.x = xScale;
        transform.localScale = scale;
    }

    public void FixedTick() {
        Vector2 direction = _mushroom.GetClosestPlayer().position - _mushroom.transform.position;
        _mushroom.Velocity = direction.normalized * _mushroom.Speed;
    }

    public void OnEnter() { }

    public void OnExit() { }
}