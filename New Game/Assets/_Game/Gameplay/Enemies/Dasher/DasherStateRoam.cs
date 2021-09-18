using UnityEngine;
using Random = UnityEngine.Random;

public class DasherStateRoam : IState {
    private DasherController _dasherController;
    private DasherData _dasherData;
    private Rigidbody2D _rb;
    private Vector2 _destination;

    private bool _roamFinished;
    public bool RoamFinished => _roamFinished;

    public DasherStateRoam(DasherController dasherController, DasherData dasherData, Rigidbody2D rb) {
        _dasherController = dasherController;
        _dasherData = dasherData;
        _rb = rb;
    }

    public void Tick() {
        if (Vector2.Distance(_rb.position, _destination) < _dasherData.roamArrivedRange) {
            _roamFinished = true;
        }
        
        Debug.DrawLine(_rb.position, _destination);
    }

    public void FixedTick() {
        Vector2 toDestination = _destination - _rb.position;
        _rb.velocity = toDestination.normalized * _dasherData.roamSpeed;
    }

    public void OnEnter() {
        _destination = ChooseDestination();
    }

    public void OnExit() {
        _rb.velocity = Vector2.zero;
        _roamFinished = false;
    }

    private Vector2 ChooseDestination() {
        return _rb.position + Random.insideUnitCircle * _dasherData.roamDestinationRadius;
    }
}
