using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSourceController : MonoBehaviour {
    public static FoodSourceController Instance { get; set; }
    [SerializeField] private GameObject foodObjectPrefab;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Feed(FaunaController fauna, Item foodItem) {
        var foodObject = Instantiate(foodObjectPrefab, transform.position, Quaternion.identity);
        foodObject.GetComponent<FoodObjectController>().Init(fauna, foodItem);
    }
}