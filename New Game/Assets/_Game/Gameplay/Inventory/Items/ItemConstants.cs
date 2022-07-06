
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemConstants : MonoBehaviour {
    public static ItemConstants Instance;
    [SerializeField] private List<Item> allItems;
    private Dictionary<String, Item> itemIdToScriptableObject;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        itemIdToScriptableObject = new Dictionary<string, Item>();
        
        foreach(Item item in allItems) {
            itemIdToScriptableObject.Add(item.Id, item);
        }
    }
    
    public static Item ItemIdToScriptableObject(String id) {
        if (!Instance.itemIdToScriptableObject.TryGetValue(id, out var result)) {
            Debug.LogError($"ItemConstants was unable to load item with id {id}!");
        }
        return result;
    }
}