using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearRopeController : RopeController {
    [SerializeField] private SpearBarbController _spearBarbController;

    private void Start() {
        base.Start();
        _spearBarbController.onHitSomething += Attach;
    }
}
