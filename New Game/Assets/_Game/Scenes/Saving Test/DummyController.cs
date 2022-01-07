using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DummyController : MonoBehaviour {
    private Vector3 _position;
    [SerializeField] private string _uuid;

    public void Init(Vector3 position, String uuid) {
        _position = position;
        _uuid = uuid;
    }

    public Vector3 GetPosition() {
        return _position;
    }

    public string GetUuid() {
        return _uuid;
    }
}
