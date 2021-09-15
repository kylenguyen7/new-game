
using System;
using _Common;
using Unity.Mathematics;
using UnityEngine;

public class DevTools : MonoBehaviour {
    [SerializeField] private GameObject _itemPrefab;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ItemController item = Instantiate(_itemPrefab, KaleUtils.GetMousePosWorldCoordinates(), Quaternion.identity)
                .GetComponent<ItemController>();
            
            item.Scatter();
        }
    }
}