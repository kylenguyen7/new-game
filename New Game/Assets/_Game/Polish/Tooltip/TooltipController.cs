using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    [SerializeField] private Vector2 cursorOffset;
    [SerializeField] private GameObject tooltipFrame;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subtitle;
    public static TooltipController Instance;
    private RectTransform _rectTransform;
    
    public String Title {
        get => title.text;
        set {
            title.text = value;
            tooltipFrame.SetActive(value != "");
        }
        
    }

    public String Subtitle {
        get => subtitle.text;
        set {
            subtitle.text = value;
        }
    }
    
    public void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        _rectTransform = GetComponent<RectTransform>();
        Instance = this;
    }

    public void Update() {
        _rectTransform.anchoredPosition = Input.mousePosition + (Vector3)cursorOffset;
    }
}
