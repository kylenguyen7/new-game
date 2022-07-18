using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1, menuName = "Apothecary/Item")]
public class Item : ScriptableObject {
    public string Id;
    public Sprite Sprite;
    public int MaxStackSize;
    
    // TODO: deprecate this
    public GameObject WorldObjectPrefab;

    public enum ItemType {
        DEFAULT,
        TINCTURE,
        TOOL,
        WORLD_OBJECT,
        BITLET,
        SEED,
        NULL
    }

    public ItemType Type;

    public String AssociatedId;
    
    // TODO: deprecate tags
    public List<String> Tags;

    public bool ContainsTag(String tag) {
        foreach(var t in Tags) {
            if (t == tag) return true;
        }

        return false;
    }

    public virtual void OnSelect() { }
    public virtual void OnDeselect() { }
    
    public virtual void OnClick() { }
}