using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pullable : MonoBehaviour {
    [SerializeField] private float _pullPower;
    public float PullPower => _pullPower;
    
    private List<RopeController> _ropeControllers = new List<RopeController>();

    public Vector2 CalculatePull() {
        Vector2 pull = Vector2.zero;
        foreach (RopeController ropeController in _ropeControllers) {
            pull += ropeController.GetPull(this);
        }
        
        return pull;
    }

    public void AddRope(RopeController ropeController) {
        _ropeControllers.Add(ropeController);
    }

    public void RemoveRope(RopeController ropeController) {
        _ropeControllers.Remove(ropeController);
    }
}
