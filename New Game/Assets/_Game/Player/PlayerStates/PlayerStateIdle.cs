using UnityEditor.Animations;
using UnityEngine;

namespace _Game.Player.PlayerStates {
    public class PlayerStateIdle : IState {
        private Animator _animator;
        public PlayerStateIdle(Animator animator) {
            _animator = animator;
        }
        public void Tick() { }
        public void FixedTick() { }

        public void OnEnter() { _animator.SetTrigger("idle"); }

        public void OnExit() { }
    }
}