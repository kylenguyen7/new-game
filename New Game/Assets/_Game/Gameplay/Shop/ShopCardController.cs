using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCardController : MonoBehaviour {
    public ShopItem shopItem;

    public void OnClick() {
        // Check gold
        var cursorItem = CursorItemSlotController.Instance.CurrentItem;
        if (Gold.Instance.Quantity <= shopItem.Cost || (cursorItem != null && cursorItem != shopItem.Item)) return;


        if (CursorItemSlotController.Instance.TryAddOne(shopItem.Item)) {
            Gold.Instance.Quantity -= shopItem.Cost;
        }
    }
}
