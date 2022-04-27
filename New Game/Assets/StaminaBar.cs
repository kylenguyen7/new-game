using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour {
    public static StaminaBar Instance;
    [SerializeField] private int startingStamina;
    [SerializeField] private Image barFilling;
    [SerializeField] private Animator barAnimator;
    [SerializeField] private TextMeshProUGUI label;
    
    private int _stamina;
    public int Stamina {
        get => _stamina;
        private set {
            float ratio = (float)value / startingStamina;
            var scale = new Vector3(1f, ratio, 1f);
            barFilling.transform.localScale = scale;
            label.text = $"{value}/{startingStamina}";
            barAnimator.SetTrigger("shake");
            _stamina = value;
        }
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Stamina = startingStamina;
        Instance = this;
    }

    public bool DepleteStamina(int amount) {
        if (Stamina < amount) {
            return false;
        }
        
        Stamina -= amount;
        return true;
    }

    public void ResetStamina() {
        Stamina = startingStamina;
    }
}
