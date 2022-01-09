using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandler : MonoBehaviour {
    public static TransitionHandler Instance;
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SaveSceneAndLoadNewScene(String sceneName) {
        foreach (var saveable in FindObjectsOfType<Saveable>()) {
            saveable.Save();
        }
        
        SceneManager.LoadScene(sceneName);
    }
}
