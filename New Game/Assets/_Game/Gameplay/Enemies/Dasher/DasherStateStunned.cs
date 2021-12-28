
using UnityEngine;

public class DasherStateStunned : IState {
    private DasherController _dasherController;
    private float _stunnedTime;
    private float _stunnedTimer;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private bool _stunFinished;
    public bool StunFinished => _stunFinished;
    
    public DasherStateStunned(DasherController dasherController, DasherData dasherData, SpriteRenderer spriteRenderer, Animator animator) {
        _stunnedTime = dasherData.stunnedTime;
        _dasherController = dasherController;
        _spriteRenderer = spriteRenderer;
        _animator = animator;
    }

    public void Tick() {
        if (_stunnedTimer < 0) {
            _stunFinished = true;
        }
        _stunnedTimer -= Time.deltaTime;
    }

    public void FixedTick() {
        _dasherController.Velocity = Vector2.zero;
    }

    public void OnEnter() {
        _stunFinished = false;
        ResetStunnedTime();
        
        _dasherController.OnDamagedCallback += ResetStunnedTime;
    }

    public void OnExit() {
        _dasherController.OnDamagedCallback -= ResetStunnedTime;
    }

    private void ResetStunnedTime() {
        _stunnedTimer = _stunnedTime;
        _animator.SetTrigger("hurt");
    }
}