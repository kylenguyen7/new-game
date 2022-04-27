
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

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
            SaveData.Instance.ManualSave();
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            Gold.Instance.Quantity += 25;
        }
    }
}