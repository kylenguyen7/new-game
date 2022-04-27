using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gold : Saveable {
    public static Gold Instance;
    [SerializeField] private TextMeshProUGUI label;
    
    private int _quantity;
    public int Quantity {
        get => _quantity;
        set {
            label.text = $"{value} gold";
            _quantity = value;
        }
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    protected override void Load() {
        Quantity = SaveData.Instance.Gold;
    }

    public override void Save() {
        SaveData.Instance.Gold = Quantity;
    }
}
