using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class RestrictedItemSlotController : ItemSlotController, IPointerClickHandler {
    [SerializeField] private String tag;
    
    public void OnPointerClick(PointerEventData eventData) {
        if (!MenuManager.Instance.HasActiveMenu) return;

        var cursor = CursorItemSlotController.Instance.GetComponent<ItemSlotController>();
        
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (cursor.CurrentItem == null || (cursor.CurrentItem != CurrentItem && cursor.CurrentItem.ContainsTag(tag))) {
                // Pick up / swap
                var cursorItem = cursor.CurrentItem;
                var cursorQuantity = cursor.Quantity;
                
                cursor.CurrentItem = CurrentItem;
                cursor.Quantity = Quantity;

                CurrentItem = cursorItem;
                Quantity = cursorQuantity;
            } else if(cursor.CurrentItem == CurrentItem) {
                int added = TryAddMany(cursor.CurrentItem, cursor.Quantity);
                cursor.TryRemoveMany(cursor.CurrentItem, added);
            }
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            if (cursor.CurrentItem == null) {
                // Cursor is empty: Split
                Item item = CurrentItem;
                int quantity = TryRemoveMany(CurrentItem, (int)Mathf.Ceil(Quantity / 2f));
                cursor.CurrentItem = item;
                cursor.Quantity = quantity;
            } else if (cursor.CurrentItem != CurrentItem && CurrentItem != null && cursor.CurrentItem.ContainsTag(tag)) {
                // Slot is different and not empty: Swap
                var cursorItem = cursor.CurrentItem;
                var cursorQuantity = cursor.Quantity;
                
                cursor.CurrentItem = CurrentItem;
                cursor.Quantity = Quantity;

                CurrentItem = cursorItem;
                Quantity = cursorQuantity;
            } else if (!IsFull() && cursor.CurrentItem.ContainsTag(tag)) {
                // Slot is empty or matching: Drop one, if possible
                var item = cursor.CurrentItem;
                cursor.TryRemoveOne(item);
                TryAddOne(item);
            }
        }
    }
}
