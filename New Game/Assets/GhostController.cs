using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {
    private PenController _penController;

    public void Init(PenController penController) {
        _penController = penController;
    }

    private void OnMouseDown() {
        _penController.ClearPen();
        Destroy(gameObject);
    }
}
