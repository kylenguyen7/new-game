using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class GestureDetector : MonoBehaviour {
    public static GestureDetector instance;
    [SerializeField] private float _minGestureLength;
    [SerializeField] private LineRenderer _lineRenderer;
    private bool _gesturing;
    private Vector2 _start;
    private Vector3[] _emptyPositions = { Vector3.zero, Vector3.zero };

    public delegate void OnGesture(Vector2 gesture);

    public event OnGesture OnGestureCallback;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }

        instance = this;
        _lineRenderer.SetPositions(_emptyPositions);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            _start = Input.mousePosition;
            _lineRenderer.SetPosition(0, KaleUtils.GetMousePosWorldCoordinates());
            _gesturing = true;
        }

        if (_gesturing) {
            // Update indicator
            _lineRenderer.SetPosition(1, KaleUtils.GetMousePosWorldCoordinates());
            Vector2 gesture = (Vector2)Input.mousePosition - _start;
            _lineRenderer.startColor = _lineRenderer.endColor = gesture.magnitude >= _minGestureLength ? Color.yellow : Color.gray;
            
            // End gesture
            if (Input.GetMouseButtonUp(1)) {
                if (gesture.magnitude >= _minGestureLength) {
                    OnGestureCallback?.Invoke(gesture.normalized);
                }
                _lineRenderer.SetPositions(_emptyPositions);
                _gesturing = false;
            }
        }
    }
}
