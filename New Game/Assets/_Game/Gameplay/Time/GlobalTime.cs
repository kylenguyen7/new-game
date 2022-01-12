using System;
using UnityEngine;

public class GlobalTime : Saveable {
    public static GlobalTime Instance;

    [Serializable]
    public struct Time {
        [SerializeField] private int hours;
        public int Hours => hours;
        [SerializeField] private int minutes;
        public int Minutes => minutes;

        public Time(int hours, int minutes) {
            if (hours > 23 || hours < 0 || minutes < 0 || minutes > 59) {
                throw new ArgumentException("Time structs can only be in the range [00:00, 23:59]");
            }

            this.hours = hours;
            this.minutes = minutes;
        }

        public override string ToString() {
            string minutesLabel = $"{minutes:D2}";

            if (hours == 12) {
                return $"12:{minutesLabel} pm";
            }

            if (hours == 0) {
                return $"12:{minutesLabel} am";
            }

            if (hours > 12) {
                return $"{hours - 12}:{minutesLabel} pm";
            }

            // hours < 12 and > 0
            return $"{hours}:{minutesLabel} am";
        }
    }

    [Serializable]
    public struct DateTime {
        [SerializeField] private Time time;
        public Time Time => time;
        [SerializeField] private int date;
        public int Date => date;

        public DateTime(Time time, int date) {
            this.time = time;
            this.date = date;
        }

        public static DateTime operator +(DateTime a, int minutes) {
            int minutesSum = a.time.Minutes + minutes;
            int hoursSum = a.time.Hours;
            int dateSum = a.date;

            while (minutesSum >= 60) {
                minutesSum -= 60;
                hoursSum++;
            }

            while (hoursSum >= 24) {
                hoursSum -= 24;
                dateSum++;
            }

            return new DateTime(new Time(hoursSum, minutesSum), dateSum);
        }

        public static int operator -(DateTime a, DateTime b) {
            int totalMinutes = 0;
            totalMinutes += (a.date - b.date) * 24 * 60;
            totalMinutes += (a.time.Hours - b.time.Hours) * 60;
            totalMinutes += (a.time.Minutes - b.time.Minutes);
            return totalMinutes;
        }

        public override string ToString() {
            return $"Day {date}\n{time.ToString()}";
        }
    }

    private DateTime _dateTime;

    public DateTime CurrentDateTime {
        get => _dateTime;
        private set {
            _dateTime = value;
            OnDateTimeChangedCallback?.Invoke(CurrentDateTime);
        }
    }

    [SerializeField] private DateTime startingTime;
    [SerializeField] private int timeUnit;
    [SerializeField] private float realTimeSecondsPerTimeUnit;
    private float _timer;

    public delegate void OnDateTimeChanged(DateTime dateTime);

    public OnDateTimeChanged OnDateTimeChangedCallback;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentDateTime = startingTime;
        _timer = realTimeSecondsPerTimeUnit;
    }

    private void Update() {
        _timer -= UnityEngine.Time.deltaTime;

        if (_timer <= 0) {
            CurrentDateTime = CurrentDateTime + timeUnit;
            _timer = realTimeSecondsPerTimeUnit;
        }
    }

    public void Sleep() {
        CurrentDateTime = new DateTime(startingTime.Time, _dateTime.Date + 1);
    }

    protected override void Load() {
        SaveData.GlobalTimeData data = SaveData.Instance.SavedGlobalTimeData;
        CurrentDateTime = new DateTime(startingTime.Time, data.Date);
    }

    public override void Save() {
        SaveData.Instance.SavedGlobalTimeData = new SaveData.GlobalTimeData(_dateTime.Date);
    }
}
