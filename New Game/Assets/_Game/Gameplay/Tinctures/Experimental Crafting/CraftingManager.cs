
using System;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
    [SerializeField] private RestrictedItemSlotController herbSlot;
    [SerializeField] private RestrictedItemSlotController activeSlot;
    [SerializeField] private CraftingResultItemSlotController resultSlot;
    
    
    [SerializeField] private Item resultItem;

    private void Start() {
        resultSlot.OnCraftCallback += () => {
            herbSlot.TryRemoveMany(herbSlot.CurrentItem, 2);
            herbSlot.TryRemoveMany(activeSlot.CurrentItem, 1);
        };
    }

    public void Update() {
        if (herbSlot.Quantity >= 2 && activeSlot.Quantity >= 1) {
            resultSlot.SetItem(resultItem, 1);
        } else {
            resultSlot.SetItem(null, 0);
        }
    }
}