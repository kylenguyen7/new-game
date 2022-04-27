using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseShopArea : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private MenuManager shop;

    public void OnPointerClick(PointerEventData eventData) {
        shop.Deactivate();
    }
}
