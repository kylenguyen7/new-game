using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunterController : MonoBehaviour, IInteractable {
    public void Interact() {
        BountyMenuManager.Instance.ActivateMenu();
    }
}
