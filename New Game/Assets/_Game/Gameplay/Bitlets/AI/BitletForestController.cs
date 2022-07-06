using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class BitletForestController : BitletController {
    [SerializeField] private Item bitletItem;

    private void OnMouseDown() {
        PickUp();
    }

    private void PickUp() {
        if (HotbarController.Instance.SelectedItem.Type == Item.ItemType.TOOL && Inventory.Instance.TryAddOne(bitletItem)) {
            HotbarController.Instance.RemoveOneFromActiveSlot();
            TextRise.Instance.CreateText("Bitlet acquired!", transform.position);
            Destroy(gameObject);
        }
    }
    
    public override Animator Animator {
        get => null;
    }
}