using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PortalController : MonoBehaviour {
    [SerializeField] private String targetSceneName;
    [SerializeField] private List<String> randomTargetSceneNames;
    [SerializeField] private Vector2 playerPositionInNewScene;
    [SerializeField] private Vector2 playerDirectionInNewScene;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            String target = targetSceneName;
            if (randomTargetSceneNames.Count != 0) {
                target = randomTargetSceneNames[Random.Range(0, randomTargetSceneNames.Count)];
            }
            TransitionHandler.Instance.SaveSceneAndLoadNewScene(target, playerPositionInNewScene, playerDirectionInNewScene);
        }
    }
}