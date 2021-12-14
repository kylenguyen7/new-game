using UnityEditor.Animations;
using UnityEngine;

namespace _Game.Player.PlayerStates {
    public class PlayerStateIdle : IState {
        private Animator _animator;
        private Rigidbody2D _rb;
        private PlayerController _playerController;
        
        public PlayerStateIdle(PlayerController playerController, Animator animator) {
            _playerController = playerController;
            _animator = animator;
        }
        public void Tick() { }

        public void FixedTick() {
            _playerController.Velocity = Vector2.zero;
        }

        public void OnEnter() {
            _animator.SetTrigger("idle");
        }

        public void OnExit() { }
    }
}