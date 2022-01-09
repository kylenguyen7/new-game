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
        UpdateTime(GlobalTime.Instance.CurrentDateTime);
    }

    private void OnEnable() {
        GlobalTime.Instance.OnDateTimeChangedCallback += UpdateTime;
    }

    private void OnDisable() {
        GlobalTime.Instance.OnDateTimeChangedCallback -= UpdateTime;
    }

    private void UpdateTime(GlobalTime.DateTime dateTime) {
        _tmp.text = dateTime.ToString();
    }
}
