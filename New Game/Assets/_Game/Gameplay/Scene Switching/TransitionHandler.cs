using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour {
    public static TransitionHandler Instance;
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeTime;
    [SerializeField] private float secondsPerFadeTick;

    private Coroutine _currentTransition;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (!blackScreen.enabled) {
            blackScreen.enabled = true;
        }
        Color col = blackScreen.color;
        col.a = 0;
        blackScreen.color = col;
    }

    public void SaveSceneAndLoadNewScene(String sceneName) {
        if (_currentTransition != null) {
            Debug.LogError($"Cannot transition to {sceneName} while another transition is occurring!");
            return;
        }
        
        _currentTransition = StartCoroutine(TransitionSequence(sceneName));
    }

    private IEnumerator TransitionSequence(String sceneName) {
        Time.timeScale = 0f;
        foreach (var saveable in FindObjectsOfType<Saveable>()) {
            saveable.Save();
        }

        Color col = blackScreen.color;
        for (float alpha = 0; alpha < 1f; alpha += secondsPerFadeTick / fadeTime) {
            col.a = alpha;
            blackScreen.color = col;
            yield return new WaitForSecondsRealtime(secondsPerFadeTick);
        }

        SceneManager.LoadScene(sceneName);
        
        for (float alpha = 1; alpha > 0; alpha -= secondsPerFadeTick / fadeTime) {
            col.a = alpha;
            blackScreen.color = col;
            yield return new WaitForSecondsRealtime(secondsPerFadeTick);
        }
        
        Time.timeScale = 1;
        _currentTransition = null;
    }
}
