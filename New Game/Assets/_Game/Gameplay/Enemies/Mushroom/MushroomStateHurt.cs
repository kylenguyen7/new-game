
using UnityEngine;

public class MushroomStateHurt : IState {
    private MushroomController _mushroom;
    private Animator _animator;
    
    private bool _done;
    public bool Done => _done;

    private float _timer;

    public MushroomStateHurt(MushroomController mushroom) {
        _mushroom = mushroom;
    }
    
    public void Tick() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) {
            _done = true;
        }
    }

    public void FixedTick() { }

    public void OnEnter() {
        _done = false;
        _timer = _mushroom.HurtTime;
        _mushroom.Velocity = Vector2.zero;
    }

    public void OnExit() { }
}