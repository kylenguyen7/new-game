using UnityEngine;

namespace _Game.Player.PlayerStates {
    public class PlayerStateMoving : IState {
        private PlayerController _playerController;
        private Rigidbody2D _rb;

        private Vector2 _direction;

        public PlayerStateMoving(PlayerController playerController) {
            _playerController = playerController;
            _rb = playerController.GetComponent<Rigidbody2D>();
        }

        public void Tick() {
            _direction = Input.GetAxisRaw("Horizontal") * Vector2.right + Input.GetAxisRaw("Vertical") * Vector2.up;
            _direction.Normalize();
        }
        
        public void FixedTick() {
            _rb.velocity = _direction.normalized * _playerController.Speed;
        }

        public void OnEnter() { }

        public void OnExit() {
            _rb.velocity = Vector2.zero;
        }
    }
}