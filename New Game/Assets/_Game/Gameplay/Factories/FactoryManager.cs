using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FactoryManager : Saveable {
    [SerializeField] private FactoryConstants factoryConstants;
    [SerializeField] private GameObject factoryPrefab;
    private List<FactoryController> _factories = new List<FactoryController>();

    protected override void Load() {
        Debug.Log($"FactoryManager loaded {SaveData.Instance.Factories.Count} factories.");
        foreach (SaveData.FactoryData factoryData in SaveData.Instance.Factories) {
            CreateAndRegisterFactory(factoryData);
        }
    }

    public override void Save() {
        List<SaveData.FactoryData> factoryDataList = new List<SaveData.FactoryData>();
        
        foreach(FactoryController factory in _factories) {
            SaveData.FactoryData factoryData =
                new SaveData.FactoryData(factory.transform.position, factory.GetStatus(), factory.GetStartTime(), factory.GetFactoryType());
            factoryDataList.Add(factoryData);
        }

        SaveData.Instance.Factories = factoryDataList;
        Debug.Log($"FactoryManager saved {factoryDataList.Count} factories.");
    }

    /**
     * Creates a new factory and places it into the save/load lifecycle. That factory will be saved
     * when the scene is exited, and loaded when the scene is reloaded. Don't create a factory in a different
     * way, because it will not persist.
     */
    public void CreateAndRegisterFactory(SaveData.FactoryData factoryData) {
        var factoryController = Instantiate(factoryPrefab).GetComponent<FactoryController>();
        factoryController.Init(factoryData, factoryConstants.GetFactoryInfo(factoryData.FactoryType));
        _factories.Add(factoryController);
    }

    public void CreateNewFactoryLite(FactoryConstants.FactoryType factoryType) {
        SaveData.FactoryData data = new SaveData.FactoryData(new Vector2(_factories.Count * 3, 2), FactoryController.Status.empty, new GlobalTime.DateTime(), factoryType);
        CreateAndRegisterFactory(data);
    }
}
