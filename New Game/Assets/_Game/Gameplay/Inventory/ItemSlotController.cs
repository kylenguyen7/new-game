using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotController : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI quantityLabel;
    
    public delegate void OnItemChanged(Item item);

    public OnItemChanged OnItemChangedCallback;

    public void OnPointerClick(PointerEventData eventData) {
        if (!MenuManager.Instance.HasActiveMenu) return;

        var cursor = CursorItemSlotController.Instance.GetComponent<ItemSlotController>();
        
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (cursor.CurrentItem == null || cursor.CurrentItem != CurrentItem) {
                // Pick up / swap
                var cursorItem = cursor.CurrentItem;
                var cursorQuantity = cursor.Quantity;
                
                cursor.CurrentItem = CurrentItem;
                cursor.Quantity = Quantity;

                CurrentItem = cursorItem;
                Quantity = cursorQuantity;
            } else {
                int added = TryAddMany(cursor.CurrentItem, cursor.Quantity);
                cursor.TryRemoveMany(cursor.CurrentItem, added);
            }
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            if (cursor.CurrentItem == null) {
                // Cursor is empty: Split
                int quantity = TryRemoveMany(CurrentItem, (int)Mathf.Ceil(Quantity / 2f));
                cursor.CurrentItem = CurrentItem;
                cursor.Quantity = quantity;
            } else if (cursor.CurrentItem != CurrentItem && CurrentItem != null) {
                // Slot is different and not empty: Swap
                var cursorItem = cursor.CurrentItem;
                var cursorQuantity = cursor.Quantity;
                
                cursor.CurrentItem = CurrentItem;
                cursor.Quantity = Quantity;

                CurrentItem = cursorItem;
                Quantity = cursorQuantity;
            } else if (!IsFull()) {
                // Slot is empty or matching: Drop one, if possible
                var item = cursor.CurrentItem;
                cursor.TryRemoveOne(item);
                TryAddOne(item);
            }
        }
    }
    
    
    private Item _currentItem;
    public Item CurrentItem {
        get => _currentItem;
        set {
            if (value == null) {
                itemImage.enabled = false;
            } else {
                itemImage.enabled = true;
                itemImage.sprite = value.Sprite;
            }
            
            OnItemChangedCallback?.Invoke(value);
            _currentItem = value;
        }
    }

    private int _quantity;
    public int Quantity {
        get => _quantity;
        set {
            quantityLabel.text = value == 0 ? "" : $"{value}";
            _quantity = value;
        }
    }

    public bool TryAddOne(Item item) {
        // Add to an empty slot
        if (IsEmpty()) {
            CurrentItem = item;
            Quantity = 1;
            return true;
        }

        // Try to add to slot, if possible (item must match and slot must not be full)
        if (CurrentItem != item) return false;
        if (IsFull()) return false;

        Quantity += 1;
        return true;
    }

    public int TryAddMany(Item item, int quantity) {
        // Add to an empty slot
        if (IsEmpty()) {
            CurrentItem = item;
            Quantity = Mathf.Min(quantity, CurrentItem.MaxStackSize);
            return Quantity;
        }
        
        if (CurrentItem != item) return 0;
        int add = Mathf.Min(GetRemainingSpace(), quantity);
        Quantity += add;
        return add;
    }

    public bool TryRemoveOne(Item item) {
        if (CurrentItem != item) {
            return false;
        }

        Quantity -= 1;
        if (IsEmpty()) {
            CurrentItem = null;
        }

        return true;
    }

    public int TryRemoveMany(Item item, int quantity) {
        if (CurrentItem != item) {
            return 0;
        }

        int toRemove = Mathf.Min(quantity, Quantity);

        Quantity -= toRemove;
        if (IsEmpty()) {
            CurrentItem = null;
        }

        return toRemove;
    }

    private int GetRemainingSpace() {
        return CurrentItem.MaxStackSize - Quantity;
    }
    
    private bool IsFull() {
        return CurrentItem != null && GetRemainingSpace() == 0;
    }

    private bool IsEmpty() {
        return Quantity == 0;
    }
}
