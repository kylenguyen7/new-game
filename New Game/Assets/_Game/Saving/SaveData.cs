using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DateTime = GlobalTime.DateTime;

public class SaveData : MonoBehaviour {
    public static SaveData Instance;

    // Serialized fields
    [SerializeField] private List<FactoryData> factories = new List<FactoryData>();

    public List<FactoryData> Factories {
        get => factories;
        set => factories = value;
    }

    [SerializeField] private GlobalTimeData globalTimeData;

    public GlobalTimeData SavedGlobalTimeData {
        get => globalTimeData;
        set => globalTimeData = value;
    }

    [SerializeField] private InventoryData inventoryData;

    public InventoryData SavedInventoryData {
        get => inventoryData;
        set => inventoryData = value;
    }

    [Serializable]
    public struct FactoryData {
        [SerializeField] private Vector2 location;
        public Vector2 Location => location;
        
        [SerializeField] private FactoryController.Status status;
        public FactoryController.Status Status => status;
        
        [SerializeField] private DateTime startTime;
        public DateTime StartTime => startTime;
        
        public FactoryData(Vector2 location, FactoryController.Status status, DateTime startTime) {
            this.location = location;
            this.status = status;
            this.startTime = startTime;
        }
    }

    [Serializable]
    public struct GlobalTimeData {
        [SerializeField] private int date;
        public int Date => date;
        public GlobalTimeData(int date) {
            this.date = date;
        }
    }

    [Serializable]
    public struct InventoryData {
        [SerializeField] private int numHoney;
        public int NumHoney => numHoney;
        [SerializeField] private int numLeaf;
        public int NumLeaf => numLeaf;

        public InventoryData(int numHoney, int numLeaf) {
            this.numHoney = numHoney;
            this.numLeaf = numLeaf;
        }
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Load data from file once at application start.
        string json = KaleSaveSystem.Load();
        if(json != null) JsonUtility.FromJsonOverwrite(json, this);
    }

    /**
     * Writes save to file. Called when the player goes to sleep.
     */
    public void ManualSave() {
        KaleSaveSystem.Save(JsonUtility.ToJson(this));
    }
}