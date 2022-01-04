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

    public OnTimeChanged OnTimeChangedCallback;

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

    public static string GetTime(int time) {
        int hours = (time / 6) % 24;
        int mins = 10 * (time % 6);

        // Time starts at 6:00 am
        string hoursLabel = ((6 + hours) % 12).ToString();
        if (hoursLabel.Equals("0")) {
            hoursLabel = "12";
        }
        string minsLabel = $"{mins:D2}";
        
        // am is if hours is in [0, 6) or [18, 30)
        // am is if (hours % 24) is in [0, 6) or [18, 24)
        bool am = hours < 6 || hours >= 18;
        String meridian = am ? "am" : "pm";

        string timeLabel = $"{hoursLabel}:{minsLabel} {meridian}";
        return timeLabel;
    }
}
