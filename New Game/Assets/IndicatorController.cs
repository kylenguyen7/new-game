using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour {
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite redIndicator;
    [SerializeField] private Sprite greenIndicator;

    public enum IndicatorMode {
        OFF,
        RED,
        GREEN,
    }
    
    public void SetMode(IndicatorMode mode) {
        switch (mode) {
            case IndicatorMode.OFF:
                sprite.sprite = null;
                break;
            case IndicatorMode.RED:
                sprite.sprite = redIndicator;
                break;
            case IndicatorMode.GREEN:
                sprite.sprite = greenIndicator;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }
}
