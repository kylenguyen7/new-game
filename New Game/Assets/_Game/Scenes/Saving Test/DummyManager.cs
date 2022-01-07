using System;
using System.Collections.Generic;
using System.Text;
using _Common;
using UnityEngine;

public class DummyManager : MonoBehaviour {
    [Serializable]
    public struct DummyData {
        public Vector3 Position;
        public string Uuid;
    }
    
    [SerializeField] private GameObject _dummyPrefab;
    
    private int _counter;
    
    private List<DummyController> _dummies = new List<DummyController>();
    
    private void Start() {
        Load();
    }

    private void OnDestroy() {
        Save();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Vector2 mousePos = KaleUtils.GetMousePosWorldCoordinates();
            CreateAndRegisterDummy(mousePos, _counter.ToString());
        }
    }

    private void CreateAndRegisterDummy(Vector3 pos, String uuid) {
        var dummy = Instantiate(_dummyPrefab, pos, Quaternion.identity).GetComponent<DummyController>();
        dummy.Init(pos, uuid);
        RegisterDummy(dummy);
        _counter++;
    }

    private void RegisterDummy(DummyController dummy) {
        _dummies.Add(dummy);
    }

    private void Save() {
        List<DummyData> dummyDataList = new List<DummyData>();
        foreach (DummyController dummy in _dummies) {
            DummyData dummyData;
            dummyData.Position = dummy.GetPosition();
            dummyData.Uuid = dummy.GetUuid();
            dummyDataList.Add(dummyData);
        }

        DummySaveData.Instance.dummies = dummyDataList;
        Debug.Log($"Dummy Manager saved Dummy Data list.");
    }

    private void Load() {
        foreach (DummyData dummyData in DummySaveData.Instance.dummies) {
            CreateAndRegisterDummy(dummyData.Position, dummyData.Uuid);
        }
        
        Debug.Log($"Dummy Manager loaded Dummy Data list.");
    }
}