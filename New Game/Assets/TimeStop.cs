using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEngine;

public class TimeStop : MonoBehaviour {
    public static TimeStop _instance;
    private Coroutine _coroutine;
    private void Awake() {
        if (_instance) {
            Debug.LogWarning($"Found more than one instance of singleton: {_instance}");
            Destroy(this);
            return;
        }
        _instance = this;
    }
    
    public void StopTime(float delay) {
        Time.timeScale = 0f;

        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(RestoreTime(delay));
    }
    
    private IEnumerator RestoreTime(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        Debug.LogWarning("Time restored to 1.");
        Debug.LogWarning(FindObjectsOfType<TimeStop>().Length);
        Time.timeScale = 1f;
    }
}
