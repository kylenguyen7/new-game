using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;
    [SerializeField] private TextMeshProUGUI _tmp;

    public int NumHoney;
    public int NumLeaf;
    
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
                NumHoney += quantity;
                break;
            case "Leaf":
                NumLeaf += quantity;
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
        _tmp.text = $"Honey: {NumHoney}\nLeaves: {NumLeaf}";
    }

    private bool RemoveItem(String item, int quantity) {
        if (item == "Honey") {
            if (NumHoney < quantity) {
                return false;
            }

            NumHoney -= quantity;
            return true;
        }
        
        if (item == "Leaf") {
            if (NumLeaf < quantity) {
                return false;
            }

            NumLeaf -= quantity;
            return true;
        }

        Debug.LogWarning($"Couldn't find item of name {item}");
        return false;
    }
}
