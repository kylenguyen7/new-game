using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEngine;

public class CursorItemSlotController : ItemSlotController {
    [SerializeField] private Vector2 cursorOffset;
    public static CursorItemSlotController Instance;
    private RectTransform _rectTransform;
    
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
