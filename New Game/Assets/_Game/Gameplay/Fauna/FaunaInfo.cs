using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 5, menuName = "Apothecary/FaunaInfo")]
public class FaunaInfo : ScriptableObject {
    public int[] XpStages;
    public Sprite[] Stages;
    
    public Item[] FoodList;
    public int[] FoodXpValues;
}
