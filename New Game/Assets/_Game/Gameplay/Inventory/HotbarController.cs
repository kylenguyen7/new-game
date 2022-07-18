using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.Design;
using UnityEngine;

public class HotbarController : MonoBehaviour {
    [SerializeField] private List<HotbarSlotController> hotbarSlots;
    [SerializeField] private Vector2 topPosition;
    [SerializeField] private Vector2 bottomPosition;
    
    public static HotbarController Instance;
    private RectTransform _rectTransform;

    private int _selectedIndex;
    private int SelectedIndex {
        get => _selectedIndex;
        set {
            HotbarSlotController oldSlot = hotbarSlots[_selectedIndex];
            oldSlot.SetActive(false);
            oldSlot.OnItemChangedCallback -= UpdateItemActions;

            _selectedIndex = (value + hotbarSlots.Count * (Mathf.Abs(value) / hotbarSlots.Count + 1)) % hotbarSlots.Count;
            HotbarSlotController newSlot = hotbarSlots[_selectedIndex];
            newSlot.SetActive(true);
            newSlot.OnItemChangedCallback += UpdateItemActions;

            UpdateItemActions(oldSlot.CurrentItem, newSlot.CurrentItem);
        }
    }

    public Item SelectedItem => hotbarSlots[SelectedIndex].CurrentItem;

    public Item.ItemType SelectedItemType {
        get {
            if (SelectedItem == null) return Item.ItemType.NULL;
            return SelectedItem.Type;
        }
    }

    /**
     * Reacts to an item in the hotbar being selected; e.g. when selecting objects of type WORLD_OBJECT
     * notify the WorldObjectGrid to show placement indicators.
     *
     * Called when a slot is first hovered, as well as if its item changes (changes in quantity do not
     * trigger this function)
     */
    private void UpdateItemActions(Item previousItem, Item newItem) {
        if (newItem == null || newItem.Type != Item.ItemType.WORLD_OBJECT) {
            if (WorldObjectGrid.Instance != null)
                WorldObjectGrid.Instance.StopPlacing(); 
        }

        if (ToolManager.Instance != null) {
            ToolManager.Instance.UpdateTool(newItem == null ? "" : newItem.Id);
        }

        if (newItem != null && newItem.Type == Item.ItemType.WORLD_OBJECT) {
            if (WorldObjectGrid.Instance == null) return;
            WorldObjectGrid.Instance.StartPlacing(newItem);
        }
        
        if(previousItem != null) previousItem.OnDeselect();
        if(newItem != null) newItem.OnSelect(); // Eventually, move everything into this function
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        _rectTransform = GetComponent<RectTransform>();
        Instance = this;
        SelectedIndex = 0;
    }

    public void Update() {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            SelectedIndex += scroll > 0 ? 1 : -1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectedIndex = 0;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectedIndex = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectedIndex = 2;
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectedIndex = 3;
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectedIndex = 4;
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SelectedIndex = 5;
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SelectedIndex = 6;
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SelectedIndex = 7;
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SelectedIndex = 8;
        } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
            SelectedIndex = 9;
        }
    }

    public Item RemoveOneFromActiveSlot() {
        Item toRemove = hotbarSlots[_selectedIndex].CurrentItem;
        if (hotbarSlots[_selectedIndex].TryRemoveOne(toRemove)) {
            return toRemove;
        }

        return null;
    }

    public enum HotbarPosition {
        TOP,
        BOTTOM
    }
    public void SetPosition(HotbarPosition position) {
        _rectTransform.anchoredPosition = GetPosition(position);
    }

    private Vector2 GetPosition(HotbarPosition position) {
        switch (position) {
            case HotbarPosition.TOP:
                return topPosition;
            case HotbarPosition.BOTTOM:
                return bottomPosition;
            default:
                throw new ArgumentOutOfRangeException(nameof(position), position, null);
        }
    }
}
