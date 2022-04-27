using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1, menuName = "Apothecary/Item")]
public class Item : ScriptableObject {
    public string Id;
    public Sprite Sprite;
    public int MaxStackSize;
    public GameObject WorldObjectPrefab;

    public enum ItemType {
        DEFAULT,
        FOOD,
        TOOL,
        WORLD_OBJECT,
        FAUNA
    }

    public ItemType Type;
}