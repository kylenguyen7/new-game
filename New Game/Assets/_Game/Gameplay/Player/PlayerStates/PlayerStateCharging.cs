using _Common;
using UnityEngine;

public class PlayerStateCharging : IState {
    private Animator _animator;
    private PlayerController _playerController;

    public PlayerStateCharging(PlayerController playerController, Animator animator) {
        _playerController = playerController;
        _animator = animator;
    }
    public void Tick() {
        Vector2 toMouse = KaleUtils.GetMousePosWorldCoordinates() - (Vector2)_playerController.transform.position;
        _animator.SetFloat("facingX", toMouse.x);
        _animator.SetFloat("facingY", toMouse.y);
    }

    public void FixedTick() {
        _playerController.Velocity = Vector2.zero;
    }

    public void OnEnter() {
        _animator.SetTrigger("idle");
        _playerController.Flash = 1;
    }

    public void OnExit() {
        _playerController.Flash = 0;
    }
}