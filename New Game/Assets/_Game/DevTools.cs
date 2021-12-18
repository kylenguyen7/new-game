
using System;
using _Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviour {
    private void Update() {

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            TimeStop._instance.StopTime();
        }
    }
}