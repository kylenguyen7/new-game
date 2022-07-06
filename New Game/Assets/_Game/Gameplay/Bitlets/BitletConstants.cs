

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BitletConstants : MonoBehaviour {
    public static BitletConstants Instance;
    [SerializeField] private RuntimeAnimatorController happyBitletAnimator;
    [SerializeField] private RuntimeAnimatorController sadBitletAnimator;
    [SerializeField] private RuntimeAnimatorController headacheBitletAnimator;

    public void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private static readonly List<String> Names = new List<String>{"Kyle", "Brandon", "Star", "Andy", "Liz", "Yvonne", "Ethan"};

    public static String GetRandomName() {
        return Names[Random.Range(0, Names.Count)];
    }

    public static RuntimeAnimatorController BitletTypeToAnimator(BitletType type) {
        switch (type) {
            case BitletType.HAPPY:
                return Instance.happyBitletAnimator;
            case BitletType.SAD:
                return Instance.sadBitletAnimator;
            case BitletType.HEADACHE:
                return Instance.headacheBitletAnimator;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}