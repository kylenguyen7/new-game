using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class FactoryController : MonoBehaviour {
    
    public enum Status {
        empty,
        working,
        finished
    }
    
    [SerializeField] private FactoryData _factoryData;

    // Factory state
    private Status _status = Status.empty;
    private int _loadTime;
    
    // Sprite
    private SpriteRenderer _spriteRenderer;
    
    // Hovering
    [SerializeField] private Vector2 _pointerOffset;
    private bool _hovered;

    public Status GetStatus() => _status;
    public int GetStartTime() => _loadTime;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start() {
        UpdateSprite();
    }

    private void Update() {
        if (_hovered) {
            if (Input.GetMouseButtonDown(0))
                AttemptLoad();
            if (Input.GetMouseButtonDown(1))
                AttemptHarvest();
        }
    }
    
    private void OnMouseEnter() {
        _hovered = true;
        Cursor.SetCursor(Resources.Load<Texture2D>("Pointer"), _pointerOffset, CursorMode.Auto);
    }

    private void OnMouseExit() {
        _hovered = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnEnable() {
        GlobalTime.Instance.OnTimeChangedCallback += UpdateWorkingStatus;
    }

    private void OnDisable() {
        GlobalTime.Instance.OnTimeChangedCallback -= UpdateWorkingStatus;
    }

    public void AttemptLoad() {
        if (_status != Status.empty) {
            Debug.Log("This factory is not empty!");
            return;
        }

        if (Inventory.Instance.RemoveItemAndUpdateLabel(_factoryData.Item.Name, _factoryData.Input)) {
            Load();
        }
        // TODO: remove
        else {
            Debug.Log("Not sufficient quantity to load!");
        }
    }

    private void Load() {
        _loadTime = GlobalTime.Instance.CurrentTime;
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
        Inventory.Instance.AddItemAndUpdateLabel(_factoryData.Item.Name, _factoryData.Output);
        UpdateStatus(Status.empty);
    }

    private void UpdateWorkingStatus(int time) {
        // Time changes should only affect working factories
        if (_status != Status.working) {
            return;
        }

        if (time - _loadTime > _factoryData.Duration) {
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
                _spriteRenderer.sprite = _factoryData.EmptySprite;
                break;
            case Status.finished:
                _spriteRenderer.sprite = _factoryData.FinishedSprite;
                break;
            case Status.working:
                _spriteRenderer.sprite = _factoryData.WorkingSprite;
                break;
        }
    }
}