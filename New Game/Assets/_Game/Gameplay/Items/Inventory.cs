using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : Saveable {
    public static Inventory Instance;
    [SerializeField] private TextMeshProUGUI _tmp;

    private int _numHoney;
    private int _numLeaf;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void AddItem(String item, int quantity) {
        switch (item) {
            case "Honey":
                _numHoney += quantity;
                break;
            case "Leaf":
                _numLeaf += quantity;
                break;
            default:
                Debug.LogWarning($"Couldn't find item of name {item}");
                break;
        }
    }

    public void AddItemAndUpdateLabel(String item, int quantity) { 
        AddItem(item, quantity);
        UpdateLabel();
    }

    public bool RemoveItemAndUpdateLabel(String item, int quantity) {
        bool result = RemoveItem(item, quantity);
        UpdateLabel();
        return result;
    }

    private void UpdateLabel() {
        _tmp.text = $"Honey: {_numHoney}\nLeaves: {_numLeaf}";
    }

    private bool RemoveItem(String item, int quantity) {
        if (item == "Honey") {
            if (_numHoney < quantity) {
                return false;
            }

            _numHoney -= quantity;
            return true;
        }
        
        if (item == "Leaf") {
            if (_numLeaf < quantity) {
                return false;
            }

            _numLeaf -= quantity;
            return true;
        }

        Debug.LogWarning($"Couldn't find item of name {item}");
        return false;
    }

    protected override void Load() {
        SaveData.InventoryData data = SaveData.Instance.SavedInventoryData;
        _numHoney = data.NumHoney;
        _numLeaf = data.NumLeaf;
        UpdateLabel();
    }

    public override void Save() {
        SaveData.Instance.SavedInventoryData = new SaveData.InventoryData(_numHoney, _numLeaf);
    }
}
