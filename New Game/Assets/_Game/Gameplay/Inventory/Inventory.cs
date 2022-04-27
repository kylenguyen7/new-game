using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Saveable {
    public static Inventory Instance;
    [SerializeField] List<ItemSlotController> inventorySlots;

    [SerializeField] private Item honeyItem;
    [SerializeField] private Item leafItem;
    [SerializeField] private Item fenceItem;
    [SerializeField] private Item leafFactoryItem;
    [SerializeField] private Item honeyFactoryItem;
    [SerializeField] private Item whiteLilyItem;
    [SerializeField] private Item smallPenItem;
    [SerializeField] private Item snailItem;
    [SerializeField] private Item fernItem;
    [SerializeField] private Item snailFoodItem;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            TryAddOne(whiteLilyItem);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            TryAddOne(honeyItem);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            TryAddOne(leafItem);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            TryAddOne(smallPenItem);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            TryAddOne(snailItem);
        }
        
        if (Input.GetKeyDown(KeyCode.T)) {
            TryAddOne(Input.GetKey(KeyCode.LeftControl) ? leafFactoryItem : honeyFactoryItem);
        }
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
            inventorySlots[i].CurrentItem = ItemIdToScriptableObject(slot.Id);
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
    
    public Item ItemIdToScriptableObject(String id) {
        switch (id) {
            case "honey": {
                return honeyItem;
            }
            case "leaf": {
                return leafItem;
            }
            case "fence": {
                return fenceItem;
            }
            case "leaf_factory": {
                return leafFactoryItem;
            }
            case "honey_factory": {
                return honeyFactoryItem;
            }
            case "white_lily": {
                return whiteLilyItem;
            }
            case "small_pen": {
                return smallPenItem;
            }
            case "snail": {
                return snailItem;
            }
            case "fern": {
                return fernItem;
            }
            case "snail_food": {
                return snailFoodItem;
            }
            case "": {
                return null;
            }
        }
        
        Debug.LogError($"Inventory was unable to load item with id {id}!");
        return null;
    }
}
