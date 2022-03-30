using UnityEngine;
using Random = UnityEngine.Random;

public class CherryStateActive : IState {
    private readonly CherryController _cherry;
    private readonly Animator _animator;
    private readonly SpriteShake _spriteShake;

    private float _cooldownTimer;
    private float _chargeTimer;
    private bool _charging;

    public CherryStateActive(CherryController cherry, Animator animator, SpriteShake shake) {
        _cherry = cherry;
        _animator = animator;
        _spriteShake = shake;
        _cooldownTimer = Random.Range(0f, _cherry.Cooldown);
        _chargeTimer = cherry.ChargeTime;
    }
        
    public void Tick() {
        if (_charging) {
            if (_chargeTimer <= 0) {
                Fire();
            }
            _chargeTimer -= Time.deltaTime;
        }
        else {  // Cooldown
            if (_cooldownTimer <= 0) {
                StartCharge();
            }
            _cooldownTimer -= Time.deltaTime;
        }
    }

    private void StartCharge() {
        _animator.SetTrigger("charging");
        _charging = true;
        _chargeTimer = _cherry.ChargeTime;
        _spriteShake.StartShake();
    }

    private void Fire() {
        _cherry.Fire();
        _charging = false;
        _cooldownTimer = _cherry.Cooldown;
        _spriteShake.PauseShake();
    }

    public void FixedTick() { }

    public void OnEnter() {
        _cooldownTimer = Random.Range(0f, _cherry.Cooldown);
        _charging = false;
    }

    public void OnExit() {
        _spriteShake.PauseShake();
    }
}