using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
public class FactoryController : MonoBehaviour {
    
    public enum Status {
        empty,
        working,
        finished
    }
    
    [SerializeField] private FactoryData _factoryData;
    [SerializeField] private Vector2 _pointerOffset;

    private Status _status = Status.empty;
    private int _startTime;
    private bool _hovered;
    
    public Status GetStatus() => _status;
    public int GetStartTime() => _startTime;

    private void OnMouseEnter() {
        _hovered = true;
        Cursor.SetCursor(Resources.Load<Texture2D>("Pointer"), _pointerOffset, CursorMode.Auto);
    }

    private void OnMouseExit() {
        _hovered = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Start() {
        throw new NotImplementedException();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _hovered) {
            Debug.Log("Mouse click detected!");
        }
    }

    public void Load() {
        if (_status != Status.empty) {
            Debug.Log("This factory is not empty!");
            return;
        }

        if (Inventory.Instance.RemoveItem(_factoryData.Item.Name, _factoryData.Input)) {
            _status = Status.working;
        }
        // TODO: remove
        else {
            Debug.Log("Not sufficient quantity to load!");
        }
    }

    public void Harvest() {
        if (_status != Status.finished) {
            Debug.Log("This factory is not harvestable!");
            return;
        }

        Inventory.Instance.AddItem(_factoryData.Item.Name, _factoryData.Output);
    }
}