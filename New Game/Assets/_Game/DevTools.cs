
using System;
using _Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        var axe = ItemConstants.ItemIdToScriptableObject("hoe");
        var pickaxe = ItemConstants.ItemIdToScriptableObject("pickaxe");
        var wateringCan = ItemConstants.ItemIdToScriptableObject("watering_can");
        if (Inventory.Instance.GetCount(axe) == 0) {
            Inventory.Instance.TryAddOne(axe);
        }
        
        if (Inventory.Instance.GetCount(pickaxe) == 0) {
            Inventory.Instance.TryAddOne(pickaxe);
        }
        
        if (Inventory.Instance.GetCount(wateringCan) == 0) {
            Inventory.Instance.TryAddOne(wateringCan);
        }
        
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("HouseScene");
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
            SaveData.Instance.ManualSave();
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            Gold.Instance.Quantity += 25;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Inventory.Instance.TryAddOne(ItemConstants.ItemIdToScriptableObject("lasso"));
        }
    }
}