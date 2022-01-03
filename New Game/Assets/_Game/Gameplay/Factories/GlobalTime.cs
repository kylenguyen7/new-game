using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTime : MonoBehaviour {
    public static GlobalTime Instance;

    private int _time;
    public int CurrentTime => _time;
    
    [SerializeField] private float _secondsPerTimeUnit;
    private float _timer;

    public delegate void OnTimeChanged(int time);
    public static OnTimeChanged OnTimeChangedCallback;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _timer = _secondsPerTimeUnit;
    }

    private void Update() {
        _timer -= Time.unscaledDeltaTime;
        
        if (_timer <= 0) {
            _time += 1;
            OnTimeChangedCallback?.Invoke(CurrentTime);
            _timer = _secondsPerTimeUnit;
        }
    }
}
