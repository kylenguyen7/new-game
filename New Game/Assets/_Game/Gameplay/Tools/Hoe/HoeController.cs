
using System;
using _Common;
using UnityEngine;

public class HoeController : ToolBase {
    [SerializeField] private Item tilledLandItem;
    private bool recharging;

    private void OnEnable() {
        StartPlacing();
        OnAttackRechargedCallback += StartPlacing;
    }

    private void OnDisable() {
        StopPlacing();
        OnAttackRechargedCallback -= StartPlacing;
    }

    protected override bool InputTrigger => Input.GetMouseButtonDown(0);

    protected override void Fire() {
        StopPlacing();
    }

    private void StartPlacing() {
        if (WorldObjectGrid.Instance != null) 
            WorldObjectGrid.Instance.StartPlacing(tilledLandItem);
    }

    private void StopPlacing() {
        if (WorldObjectGrid.Instance != null) 
            WorldObjectGrid.Instance.StopPlacing();
    }
}