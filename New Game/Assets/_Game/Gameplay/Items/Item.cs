using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1, menuName = "Apothecary/Item")]
public class Item : ScriptableObject {
    public string Name;
    public Sprite Sprite;
}
