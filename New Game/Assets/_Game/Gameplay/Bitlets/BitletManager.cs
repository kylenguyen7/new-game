using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BitletManager : Saveable {
    [SerializeField] private GameObject happyBitlet;
    [SerializeField] private GameObject sadBitlet;
    [SerializeField] private GameObject headacheBitlet;
    
    protected override void Load() {
        List<SaveData.BitletData> townBitletData = SaveData.Instance.TownBitletData;
        foreach (var data in townBitletData) {
            SpawnBitlet(data.Type, data.Name, data.X, data.Y, data.TreatmentProgress, data.LastDayTreated);
        }
    }

    public override void Save() {
        List<SaveData.BitletData> townBitletData = new List<SaveData.BitletData>();
        foreach (var bitlet in FindObjectsOfType<BitletRanchController>()) {
            var pos = bitlet.transform.position;
            SaveData.BitletData data = new SaveData.BitletData(bitlet.BitletType, bitlet.Name, pos.x, pos.y, bitlet.TreatmentProgress, bitlet.LastDayTreated);
            townBitletData.Add(data);
        }
        
        SaveData.Instance.TownBitletData = townBitletData;
    }
    
    private void SpawnBitlet(BitletType type, String name, float x, float y, int treatmentProgress, int lastDayTreated) {
        var bitlet = Instantiate(BitletTypeToPrefab(type), new Vector2(x, y), Quaternion.identity)
            .GetComponent<BitletRanchController>();
        bitlet.Init(name, treatmentProgress, lastDayTreated);
    }

    private GameObject BitletTypeToPrefab(BitletType type) {
        switch (type) {
            case BitletType.HAPPY:
                return happyBitlet;
            case BitletType.SAD:
                return sadBitlet;
            case BitletType.HEADACHE:
                return headacheBitlet;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
