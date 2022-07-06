using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour {
    public static ToolManager Instance;
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject hoe;
    [SerializeField] private GameObject wateringCan;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        var item = HotbarController.Instance.SelectedItem;
        UpdateTool(item == null ? "" : item.Id);
    }

    public void UpdateTool(string itemId) {
        switch (itemId) {
            case "pickaxe": {
                pickaxe.SetActive(true);
                hoe.SetActive(false);
                wateringCan.SetActive(false);
                break;
            }
            case "hoe": {
                pickaxe.SetActive(false);
                hoe.SetActive(true);
                wateringCan.SetActive(false);
                break;
            }
            case "watering_can": {
                pickaxe.SetActive(false);
                hoe.SetActive(false);
                wateringCan.SetActive(true);
                break;
            }
            default: {
                pickaxe.SetActive(false);
                hoe.SetActive(false);
                wateringCan.SetActive(false);
                break;
            }
        }
    }
}
