
using System;
using _Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviour {
    [SerializeField] private GameObject _itemPrefab;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ItemController item = Instantiate(_itemPrefab, KaleUtils.GetMousePosWorldCoordinates(), Quaternion.identity)
                .GetComponent<ItemController>();
            
            item.Scatter();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            TimeStop._instance.StopTime(1);
        }
    }
}