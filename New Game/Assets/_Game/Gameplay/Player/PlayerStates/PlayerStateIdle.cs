using UnityEditor.Animations;
using UnityEngine;

namespace _Game.Player.PlayerStates {
    public class PlayerStateIdle : IState {
        private Animator _animator;
        private Rigidbody2D _rb;
        private PlayerController _playerController;
        
        public PlayerStateIdle(PlayerController playerController, Rigidbody2D rb, Animator animator) {
            _rb = rb;
            _playerController = playerController;
            _animator = animator;
        }
        public void Tick() { }

        public void FixedTick() {
            _rb.velocity = Vector2.zero;
            // _rb.velocity = _playerController.CalculatePull();
        }

        public void OnEnter() {
            _animator.SetTrigger("idle");
        }

        public void OnExit() { }
    }
}