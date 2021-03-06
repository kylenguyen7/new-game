using System;
using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEngine;

public class TimeStop : MonoBehaviour {
    public static TimeStop _instance;
    private Coroutine _coroutine;
    [SerializeField] private float _delay;

private void Awake() {
        if (_instance) {
            Debug.LogWarning($"Found more than one instance of singleton: {_instance}");
            Destroy(this);
            return;
        }
        _instance = this;
    }
    
    public void StopTime() {
        Time.timeScale = 0f;

        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(RestoreTime(_delay));
    }

    public void StopTimeManual() {
        Time.timeScale = 0f;
    }

    public void ResumeTimeManual() {
        Time.timeScale = 1f;
    }
    
    private IEnumerator RestoreTime(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
    }
}
