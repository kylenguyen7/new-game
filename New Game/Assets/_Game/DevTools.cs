
using System;
using _Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("HouseScene");
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Inventory.Instance.AddItemAndUpdateLabel(Input.GetKey(KeyCode.LeftControl) ? "Leaf" : "Honey", 1);
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            FindObjectOfType<FactoryManager>()?.CreateNewFactoryLite(Input.GetKey(KeyCode.LeftControl) ? FactoryConstants.FactoryType.LEAF : FactoryConstants.FactoryType.HONEY);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
            SaveData.Instance.ManualSave();
        }
    }
}