using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FactoryController : MonoBehaviour {
    public enum Status {
        empty,
        working,
        finished
    }
    
    [SerializeField] private FactoryData _factoryData;

    private Status _status = Status.empty;
    private int _startTime;
    
    public Status GetStatus() => _status;
    public int GetStartTime() => _startTime;

    public void Load() {
        if (_status != Status.empty) {
            Debug.Log("This factory is not empty!");
            return;
        }

        if (Inventory.Instance.RemoveItem(_factoryData.Item.Name, _factoryData.Input)) {
            _status = Status.working;
        }
        // TODO: remove
        else {
            Debug.Log("Not sufficient quantity to load!");
        }
    }

    public void Harvest() {
        if (_status != Status.finished) {
            Debug.Log("This factory is not harvestable!");
            return;
        }

        Inventory.Instance.AddItem(_factoryData.Item.Name, _factoryData.Output);
    }
}

[CreateAssetMenu(order = 2, menuName = "Apothecary/FactoryData")]
public class FactoryData : ScriptableObject {
    public Item Item;
    public int Cost;
    public int Input;
    public int Output;
    public int Duration;

    public Sprite EmptySprite;
    public Sprite WorkingSprite;
    public Sprite FinishedSprite;
}