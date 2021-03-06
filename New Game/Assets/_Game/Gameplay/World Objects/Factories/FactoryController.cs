using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DateTime = GlobalTime.DateTime;

[RequireComponent(typeof(Collider2D))]
public class FactoryController : WorldObjectController {
    [Serializable]
    public enum Status {
        empty,
        working,
        finished
    }
    
    // Factory type
    [SerializeField] private FactoryInfo _factoryInfo;

    // Factory state
    private Status _status = Status.empty;
    private DateTime _startTime;
    
    // Sprite
    private SpriteRenderer _spriteRenderer;
    
    // Hovering
    // private Vector2 _pointerOffset = new Vector2(190, 60);
    private bool _hovered;

    public Status GetStatus() => _status;
    public DateTime GetStartTime() => _startTime;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // public void Init(SaveData.FactoryData factoryData, FactoryInfo factoryInfo) {
    //     transform.position = factoryData.Location;
    //     _status = factoryData.Status;
    //     _startTime = factoryData.StartTime;
    //     _factoryInfo = factoryInfo;
    //
    //     UpdateWorkingStatus(GlobalTime.Instance.CurrentDateTime);
    //     UpdateSprite();
    // }

    private void Update() {
        
        // Print JsonUtility.ToJson(this);
        
        if (_hovered) {
            if (Input.GetMouseButtonDown(0))
                AttemptLoad();
            if (Input.GetMouseButtonDown(1))
                AttemptHarvest();
        }
    }
    
    private void OnMouseEnter() {
        _hovered = true;
        // Cursor.SetCursor(Resources.Load<Texture2D>("Pointer"), _pointerOffset, CursorMode.Auto);
    }

    private void OnMouseExit() {
        _hovered = false;
        // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnEnable() {
        GlobalTime.Instance.OnDateTimeChangedCallback += UpdateWorkingStatus;
    }

    private void OnDisable() {
        GlobalTime.Instance.OnDateTimeChangedCallback -= UpdateWorkingStatus;
    }

    public void AttemptLoad() {
        if (_status != Status.empty) {
            Debug.Log("This factory is not empty!");
            return;
        }

        if (Inventory.Instance.TryRemoveOne(_factoryInfo.Item)) {
            Load();
        }
        // TODO: remove
        else {
            Debug.Log("Not sufficient quantity to load!");
        }
    }

    private void Load() {
        _startTime = GlobalTime.Instance.CurrentDateTime;
        UpdateStatus(Status.working);
    }

    public void AttemptHarvest() {
        if (_status != Status.finished) {
            Debug.Log("This factory is not harvestable!");
            return;
        }
        Harvest();
    }

    private void Harvest() {
        Inventory.Instance.TryAddOne(_factoryInfo.Item);
        UpdateStatus(Status.empty);
    }

    private void UpdateWorkingStatus(DateTime time) {
        // Time changes should only affect working factories
        if (_status != Status.working) {
            return;
        }

        if (time - _startTime > _factoryInfo.Duration) {
            UpdateStatus(Status.finished);
        }
    }

    private void UpdateStatus(Status newStatus) {
        _status = newStatus;
        UpdateSprite();
    }

    /**
     * Updates sprite based on current state, and if in the working state, the current time.
     */
    private void UpdateSprite() {
        switch (_status) {
            case Status.empty:
                _spriteRenderer.sprite = _factoryInfo.EmptySprite;
                break;
            case Status.finished:
                _spriteRenderer.sprite = _factoryInfo.FinishedSprite;
                break;
            case Status.working:
                _spriteRenderer.sprite = _factoryInfo.WorkingSprite;
                break;
        }
    }
}