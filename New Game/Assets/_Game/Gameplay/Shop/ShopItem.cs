using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 6, menuName = "Apothecary/ShopItem")]
public class ShopItem : ScriptableObject {
    public Item Item;
    public int Cost;
}
