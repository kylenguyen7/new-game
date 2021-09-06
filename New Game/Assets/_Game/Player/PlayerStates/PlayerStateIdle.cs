using UnityEditor.Animations;
using UnityEngine;

namespace _Game.Player.PlayerStates {
    public class PlayerStateIdle : IState {
        private Animator _animator;
        private Rigidbody2D _rb;
        private BowController _bowController;
        
        public PlayerStateIdle(Rigidbody2D rb, BowController bowController, Animator animator) {
            _rb = rb;
            _bowController = bowController;
            _animator = animator;
        }
        public void Tick() { }

        public void FixedTick() {
            _rb.velocity = _bowController.Pull;
        }

        public void OnEnter() { }  //_animator.SetTrigger("idle"); }

        public void OnExit() { }
    }
}