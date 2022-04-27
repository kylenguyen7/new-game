using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableFloraController : Workable {
    [SerializeField] private SpriteShake spriteShake;
    [SerializeField] private Item floraItem;
    [SerializeField] private GameObject itemPrefab;

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
        var item = Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponent<ItemController>();
        item.Init(floraItem);
    }
}
