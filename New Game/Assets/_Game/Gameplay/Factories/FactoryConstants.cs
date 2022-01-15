using System;
using UnityEngine;

[CreateAssetMenu(order = 3, menuName = "Data/FactoryConstants")]
public class FactoryConstants : ScriptableObject {
    [SerializeField] private FactoryInfo leafFactoryInfo;
    [SerializeField] private FactoryInfo honeyFactoryInfo;
    
    public enum FactoryType {
        LEAF,
        HONEY
    }

    public FactoryInfo GetFactoryInfo(FactoryType factoryType) {
        switch (factoryType) {
            case FactoryType.LEAF: return leafFactoryInfo;
            case FactoryType.HONEY: return honeyFactoryInfo;
        }
        
        Debug.LogWarning($"Could not find a factoryInfo for factory of type {factoryType}.");
        return null;
    }
}