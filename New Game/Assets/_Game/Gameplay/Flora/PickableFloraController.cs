using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickableFloraController : Workable {
    [SerializeField] private SpriteShake spriteShake;
    [SerializeField] private Item floraItem;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int minItems = 1;
    [SerializeField] private int maxItems = 1;

    public override bool CanWork() {
        return StaminaBar.Instance.Stamina >= 2;
    }

    public override string GetErrorMessage() {
        return "Not enough stamina!";
    }

    protected override void OnWorkStart() {
        spriteShake.StartShake();
    }

    protected override void OnWorkStop() {
        spriteShake.PauseShake();
    }

    protected override void OnWorkFinish() {
        StaminaBar.Instance.DepleteStamina(2);
        SpawnItem();
        TextRise.Instance.CreateText("+2 xp", _player.transform.position);
        Destroy(gameObject);
    }

    private void SpawnItem() {
        int count = Random.Range(minItems, maxItems + 1);
        for (int i = 0; i < count; i++) {
            var item = Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponent<ItemController>();
            item.Init(floraItem);
        }
    }
}
