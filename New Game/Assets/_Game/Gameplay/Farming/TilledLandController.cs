

using System;
using UnityEngine;

public class TilledLandController : WorldObjectController {
    private Item _plantedSeed;
    private int _stage;
    private int _lastDayWatered;
    
    public override void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if(HotbarController.Instance.SelectedItemType == Item.ItemType.SEED) {
                var item = HotbarController.Instance.SelectedItem;
                Init(item, 0);
                HotbarController.Instance.RemoveOneFromActiveSlot();
            }
        }
    }

    private void Init(Item seed, int stage) {
        _plantedSeed = seed;
        _stage = stage;
    }

    public override string GetMetaData() {
        return $"{_plantedSeed.Id} {_stage}";
    }

    public override void LoadMetaData(String data) {
        string[] tokens = data.Split(' ');
        Init(ItemConstants.ItemIdToScriptableObject(tokens[0]), Int32.Parse(tokens[1]));
    }
}