using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour {
    public static SaveData Instance;

    public List<FactoryData> factories = new List<FactoryData>();
    
    public struct FactoryData {
        public Vector2 Location;
        public FactoryController.Status Status;
        public int StartTime;
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