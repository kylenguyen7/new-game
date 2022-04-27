using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, menuName = "Apothecary/FactoryData")]
public class FactoryInfo : ScriptableObject {
    public Item Item;
    public int Duration;

    public Sprite EmptySprite;
    public Sprite WorkingSprite;
    public Sprite FinishedSprite;
}