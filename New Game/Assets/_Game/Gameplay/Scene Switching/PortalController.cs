using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour {
    [SerializeField] private String targetSceneName;
    
    private void OnTriggerEnter2D(Collider2D other) {
        TransitionHandler.Instance.SaveSceneAndLoadNewScene(targetSceneName);
    }
}