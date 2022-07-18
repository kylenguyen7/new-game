

using System;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotController : ItemSlotController {
    [SerializeField] private Image selector;
    
    public void SetActive(bool active) {
        selector.enabled = active;
    }

    private void Update() {
        if (CurrentItem != null && Input.GetMouseButtonDown(0)) {
            CurrentItem.OnClick();
        }
    }
}