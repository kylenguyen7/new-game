using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private Ray _ray;
    RaycastHit _hit;
    
    private void Update() {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_ray, out _hit, Mathf.Infinity)) {
            Debug.Log(_hit.collider.name);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition));
    }
}
