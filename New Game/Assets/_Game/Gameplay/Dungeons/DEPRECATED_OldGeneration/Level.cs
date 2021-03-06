using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Apothecary/Level")]
public class Level : ScriptableObject {
    public Vector2 CameraBounds;
    public GameObject LevelPrefab;
}