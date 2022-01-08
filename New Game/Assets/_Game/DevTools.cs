
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Inventory.Instance.AddItemAndUpdateLabel("Leaf", 1);
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            FindObjectOfType<PlayerController>().gameObject.transform.position = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
            SaveData.Instance.ManualSave();
        }
        
        // TODO: Add dev tool to spawn a factory to test factory persistence.
    }
}