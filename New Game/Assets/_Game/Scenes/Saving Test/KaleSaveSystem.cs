using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public static class KaleSaveSystem {
    // TODO: Change this to Application.persistentDataPath
    public static readonly string SAVE_DIR = Application.dataPath + "/Saves";
    public static readonly string SAVE_FILENAME = "/save.txt";
    
    public static void CheckDirectoryExists() {
        if (!Directory.Exists(SAVE_DIR)) {
            Directory.CreateDirectory(SAVE_DIR);
        }
    }

    public static void Save(string saveString) {
        CheckDirectoryExists();
        File.WriteAllText(SAVE_DIR + SAVE_FILENAME, saveString);
    }

    public static string Load() {
        if (File.Exists(SAVE_DIR + SAVE_FILENAME)) {
            string saveString = File.ReadAllText(SAVE_DIR + SAVE_FILENAME);
            return saveString;
        }

        return null;
    }
}
