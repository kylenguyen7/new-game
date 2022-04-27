
using System;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PenController : WorldObjectController {
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] Item snailItem;

    private String _currentFaunaId = "null";
    private FaunaController _currentFauna;

    private bool IsEmpty() {
        return _currentFaunaId == "null";
    }
    
    private bool IsDead() {
        return _currentFaunaId == "dead";
    }
    
    private void OnMouseDown() {
        if (IsEmpty() && HotbarController.Instance.SelectedItem != null && 
            HotbarController.Instance.SelectedItem.Type == Item.ItemType.FAUNA) {
            Item fauna = HotbarController.Instance.RemoveOneFromActiveSlot();
            AddFauna(fauna,0, 0, GlobalTime.Instance.CurrentDateTime.Date - 1);
        }
    }

    /**
     * Metadata format:
     * fauna_id xp last_date_fed
     */
    public override string GetMetaData() {
        if (IsEmpty() || IsDead()) {
            return _currentFaunaId;         // "null" or "dead"
        }
        
        return $"{_currentFaunaId} {_currentFauna.Xp} {_currentFauna.XpGain} {_currentFauna.LastDateFed} {_currentFauna.IsMature}";
    }
    
    /**
     * Updates status of fauna based on stored id and date.
     */
    public override void LoadMetaData(string data) {
        string[] tokens = data.Split(' ');
        string faunaId = tokens[0];

        // Pen is empty
        if (faunaId == "null") {
            _currentFaunaId = "null";
            return;
        }

        // Fauna is marked as dead, or hasn't been fed in three days
        if (faunaId == "dead" || GlobalTime.Instance.CurrentDateTime.Date - Int32.Parse(tokens[3]) >= 3 && !Boolean.Parse(tokens[4])) {
            _currentFaunaId = "dead";
            var ghost = InstantiateInFront(ghostPrefab, transform.position).GetComponent<GhostController>();
            ghost.Init(this);
            return;
        }
        
        // Fauna is alive and chillin'
        int xp = Int32.Parse(tokens[1]);
        int xpGain = Int32.Parse(tokens[2]);
        int lastDateFed = Int32.Parse(tokens[3]);
        _currentFaunaId = faunaId;
        switch (faunaId) {
            case "snail": {
                if (lastDateFed < GlobalTime.Instance.CurrentDateTime.Date) {
                    AddFauna(snailItem, xp + xpGain, 0, lastDateFed);
                } else {
                    AddFauna(snailItem, xp, xpGain, lastDateFed);
                }
                break;
            }
            default: {
                Debug.LogWarning($"PenController was unable to find a fauna with ID {data}!");
                return;
            }
        }
    }

    private void AddFauna(Item fauna, int xp, int xpGain, int lastDateFed) {
        _currentFaunaId = fauna.Id;
        _currentFauna = InstantiateInFront(fauna.WorldObjectPrefab, transform.position)
            .GetComponent<FaunaController>();
        _currentFauna.Init(xp, xpGain, lastDateFed);
    }

    private GameObject InstantiateInFront(GameObject obj, Vector3 position) {
        position.z = -1;
        return Instantiate(obj, position, Quaternion.identity);
    }

    public void ClearPen() {
        _currentFaunaId = "null";
    }
}