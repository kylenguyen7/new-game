
using System;
using UnityEngine;

[CreateAssetMenu(order = 4, menuName = "Apothecary/EnemyInfo")]
public class EnemyInfoScriptableObject : ScriptableObject {
    [SerializeField] public EnemyType Type;
    [SerializeField] public String Name;
    [SerializeField] public Sprite Sprite;
}