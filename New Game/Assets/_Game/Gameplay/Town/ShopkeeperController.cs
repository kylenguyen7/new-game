using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperController : MonoBehaviour, IInteractable {
    public void Interact() {
        BountyMenuManager.Instance.ActivateMenu();
    }
}
