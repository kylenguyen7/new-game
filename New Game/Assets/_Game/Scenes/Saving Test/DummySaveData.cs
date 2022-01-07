using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DummySaveData : MonoBehaviour {
    public static DummySaveData Instance;
    public List<DummyManager.DummyData> dummies = new List<DummyManager.DummyData>();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // Load data from file once at application start.
        string json = KaleSaveSystem.Load();
        if(json != null) JsonUtility.FromJsonOverwrite(json, this);

        DontDestroyOnLoad(gameObject);
    }
    
    private void Update() {
        // Save on sleep
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
            KaleSaveSystem.Save(JsonUtility.ToJson(this));
        }
        
        // Testing purposes: change scenes
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SceneManager.LoadScene("SavingTestScene");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SceneManager.LoadScene("SavingTestScene2");
        }
    }
}