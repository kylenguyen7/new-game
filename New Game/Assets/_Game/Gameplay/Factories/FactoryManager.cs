using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FactoryManager : Saveable {
    [SerializeField] private GameObject _factoryPrefab;
    private List<FactoryController> _factories = new List<FactoryController>();
    
    protected override void Load() {
        foreach (SaveData.FactoryData factoryData in SaveData.Instance.factories) {
            CreateAndRegisterFactory(factoryData);
        }
    }

    protected override void Save() {
        List<SaveData.FactoryData> factoryDataList = new List<SaveData.FactoryData>();
        
        foreach(FactoryController factory in _factories) {
            SaveData.FactoryData factoryData = new SaveData.FactoryData();
            factoryData.Location = factory.transform.position;
            factoryData.Status = factory.GetStatus();
            factoryData.StartTime = factory.GetStartTime();
        }

        SaveData.Instance.factories = factoryDataList;
    }

    /**
     * Creates a new factory and places it into the save/load lifecycle. That factory will be saved
     * when the scene is exited, and loaded when the scene is reloaded. Don't create a factory in a different
     * way, because it will not persist.
     */
    public void CreateAndRegisterFactory(SaveData.FactoryData factoryData) {
        var factoryController = Instantiate(_factoryPrefab).GetComponent<FactoryController>();
        factoryController.Init(factoryData);
        _factories.Add(factoryController);
    }
}
