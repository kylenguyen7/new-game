using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApothecaryTableController : Workable {
    [SerializeField] private Item fernItem;
    [SerializeField] private Item snailFoodItem;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int cost;
    [SerializeField] private SpriteShake bowlSpriteShake;
    [SerializeField] private SpriteRenderer bowlSpriteRenderer;
    [SerializeField] private Sprite emptyBowl;
    [SerializeField] private Sprite workingBowl;

    public override bool CanWork() {
        return Inventory.Instance.GetCount(fernItem) >= cost;
    }

    public override string GetErrorMessage() {
        return $"Need {cost} ferns!";
    }

    protected override void OnWorkStart() {
        bowlSpriteShake.StartShake();
        bowlSpriteRenderer.sprite = workingBowl;
    }

    protected override void OnWorkFinish() {
        var item = Instantiate(itemPrefab, transform.position, Quaternion.identity)
            .GetComponent<ItemController>();
        item.Init(snailFoodItem);
        bowlSpriteShake.PauseShake();
        bowlSpriteRenderer.sprite = emptyBowl;
        Inventory.Instance.TryRemoveMultiple(fernItem, cost);
    }

    protected override void OnWorkStop() {
        bowlSpriteShake.PauseShake();
        bowlSpriteRenderer.sprite = emptyBowl;
    }
}
