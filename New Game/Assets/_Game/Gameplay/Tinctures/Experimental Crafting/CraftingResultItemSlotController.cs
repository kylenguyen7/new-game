using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingResultItemSlotController : ItemSlotController, IPointerClickHandler {
    public delegate void OnCraft();
    public OnCraft OnCraftCallback;

    public void OnPointerClick(PointerEventData eventData) {
        if (!MenuManager.Instance.HasActiveMenu) return;

        var cursor = CursorItemSlotController.Instance.GetComponent<ItemSlotController>();

        if (eventData.button == PointerEventData.InputButton.Left) {
            if (cursor.CurrentItem == null || (cursor.CurrentItem == CurrentItem)) {
                // Pick up one
                if (cursor.TryAddOne(CurrentItem)) {
                    TryRemoveOne(CurrentItem);
                    OnCraftCallback?.Invoke();
                }
            }
        }
    }

    public void SetItem(Item item, int quantity) {
        CurrentItem = item;
        Quantity = quantity;
    }
}
