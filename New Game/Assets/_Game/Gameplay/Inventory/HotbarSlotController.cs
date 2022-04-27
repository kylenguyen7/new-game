

using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotController : ItemSlotController {
    [SerializeField] private Image selector;
    
    public void SetActive(bool active) {
        selector.enabled = active;
    }
}