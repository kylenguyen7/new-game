using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextRise : MonoBehaviour {
    public static TextRise Instance;
    [SerializeField] private GameObject textRisePrefab;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void CreateText(String text, Vector2 position) {
        var tmp = Instantiate(textRisePrefab, position, Quaternion.identity).GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = text;
    }
}
