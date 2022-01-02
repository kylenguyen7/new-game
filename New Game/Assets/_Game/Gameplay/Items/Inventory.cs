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

    public void AddItem(String item, int quantity) {
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
        
        _tmp.text = $"Honey: {NumHoney}\nLeaves: {NumLeaf}";
    }

    public bool RemoveItem(String item, int quantity) {
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
