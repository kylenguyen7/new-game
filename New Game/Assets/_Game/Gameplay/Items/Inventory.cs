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

    public void AddItem(String item) {
        if (item == "Honey") {
            NumHoney += 1;
        } else if (item == "Leaf") {
            NumLeaf += 1;
        } else {
            Debug.LogWarning($"Couldn't find item of name {item}");
        }

        _tmp.text = $"Honey: {NumHoney}\nLeaves: {NumLeaf}";
    }
}
