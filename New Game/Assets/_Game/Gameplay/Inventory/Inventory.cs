using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Saveable {
    public static Inventory Instance;
    [SerializeField] List<ItemSlotController> inventorySlots;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool TryAddOne(Item item) {
        foreach(var slot in inventorySlots) {
            if (slot.TryAddOne(item)) {
                return true;
            }
        }
        return false;
    }

    public bool TryRemoveOne(Item item) {
        foreach(var slot in inventorySlots) {
            if (slot.TryRemoveOne(item)) {
                return true;
            }
        }
        return false;
    }

    public int GetCount(Item item) {
        int inventoryCount = 0;
        foreach(var slot in inventorySlots) {
            if (slot.CurrentItem == item) {
                inventoryCount += slot.Quantity;
            }
        }
        return inventoryCount;
    }

    public bool TryRemoveMultiple(Item item, int quantity) {
        int inventoryCount = GetCount(item);
        if (inventoryCount < quantity) return false;
        
        int i = inventorySlots.Count - 1;
        while (quantity > 0) {
            quantity -= inventorySlots[i].TryRemoveMany(item, quantity);
            i--;
        }
        return true;
    }

    protected override void Load() {
        SaveData.InventoryData data = SaveData.Instance.SavedInventoryData;
        var slotData = data.SlotData;
        for (int i = 0; i < slotData.Count; i++) {
            var slot = slotData[i];
            if (slot.Id == "") continue;
            inventorySlots[i].CurrentItem = ItemConstants.ItemIdToScriptableObject(slot.Id);
            inventorySlots[i].Quantity = slot.Quantity;
        }
    }

    public override void Save() {
        List<SaveData.SlotData> slots = new List<SaveData.SlotData>();
        foreach(var slot in inventorySlots) {
            slots.Add(new SaveData.SlotData(slot.CurrentItem == null ? null : slot.CurrentItem.Id, slot.Quantity));
        }
        SaveData.Instance.SavedInventoryData = new SaveData.InventoryData(slots);
    }
}
