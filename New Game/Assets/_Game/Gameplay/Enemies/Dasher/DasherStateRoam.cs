using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DasherStateRoam : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private Vector2 _destination;

    private bool _roamFinished;
    public bool RoamFinished => _roamFinished;
    
    private Animator _animator;

    public DasherStateRoam(DasherController dasherController, DasherData dasherData, Animator animator) {
        _dasherController = dasherController;
        _dasherData = dasherData;
        _animator = animator;
    }

    public void Tick() {
        if (Vector2.Distance(_dasherController.transform.position, _destination) < _dasherData.roamArrivedRange) {
            _roamFinished = true;
        }
        
        Debug.DrawLine(_dasherController.transform.position, _destination);
    }

    public void FixedTick() {
        Vector2 toDestination = _destination - (Vector2)_dasherController.transform.position;
        _dasherController.Velocity = toDestination.normalized * _dasherData.roamSpeed;
        _animator.SetFloat("facingX", Mathf.Sign(_dasherController.Velocity.x));
    }

    public void OnEnter() {
        _destination = ChooseDestination();
    }

    public void OnExit() {
        _dasherController.Velocity = Vector2.zero;
        _roamFinished = false;
    }

    private Vector2 ChooseDestination() {
        return (Vector2)_dasherController.transform.position + Random.insideUnitCircle * _dasherData.roamDestinationRadius;
    }
}
