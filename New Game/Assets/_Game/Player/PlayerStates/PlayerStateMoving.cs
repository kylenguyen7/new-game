using UnityEngine;

namespace _Game.Player.PlayerStates {
    public class PlayerStateMoving : IState {
        private PlayerController _playerController;
        private Animator _animator;
        private Rigidbody2D _rb;

        private Vector2 _direction;

        public PlayerStateMoving(PlayerController playerController, Animator animator) {
            _playerController = playerController;
            _rb = playerController.GetComponent<Rigidbody2D>();
            _animator = animator;
        }

        public void Tick() {
            _direction = Input.GetAxisRaw("Horizontal") * Vector2.right + Input.GetAxisRaw("Vertical") * Vector2.up;
            _direction.Normalize();
        }
        
        public void FixedTick() {
            UpdateFacing();
            _rb.velocity = _direction.normalized * _playerController.Speed;
        }

        private void UpdateFacing() {
            if (_direction.y < 0) {
                _playerController.Facing = new Vector2(0, -1);
            } else if (_direction.x != 0) {
                _playerController.Facing = new Vector2(Mathf.Sign(_direction.x), 0);
            } else if(_direction.y > 0) {
                _playerController.Facing = new Vector2(0, 1);
            }
            
            _animator.SetFloat("facingX", _playerController.Facing.x);
            _animator.SetFloat("facingY", _playerController.Facing.y);
        }

        public void OnEnter() {
            _animator.SetTrigger("moving"); 
        }

        public void OnExit() {
            _rb.velocity = Vector2.zero;
        }
    }
}