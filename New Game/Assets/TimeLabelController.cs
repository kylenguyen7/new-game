using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeLabelController : MonoBehaviour {
    private TextMeshProUGUI _tmp;

    private void Awake() {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        UpdateTime(GlobalTime.Instance.CurrentTime);
    }

    private void OnEnable() {
        GlobalTime.Instance.OnTimeChangedCallback += UpdateTime;
    }

    private void OnDisable() {
        GlobalTime.Instance.OnTimeChangedCallback -= UpdateTime;
    }

    private void UpdateTime(int time) {
        _tmp.text = GlobalTime.GetTime(time);
    }
}
