using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour {
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject healthBarFilling;

    private void Update() {
        var scale = healthBarFilling.transform.localScale;

        if (player == null) {
            scale.x = 0;
        }
        else {
            scale.x = player.Hp / player.StartingHp;
        }

        healthBarFilling.transform.localScale = scale;
    }
}
