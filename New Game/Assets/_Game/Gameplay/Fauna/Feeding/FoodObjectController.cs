using UnityEngine;

public class FoodObjectController : ParabolicProjectileController {
    [SerializeField] private SpriteRenderer sprite;
    private FaunaController _targetFauna;
    private Item _foodItem;

    public void Init(FaunaController fauna, Item foodItem) {
        SetTarget(fauna.transform);
        _targetFauna = fauna;
        _foodItem = foodItem;
        sprite.sprite = foodItem.Sprite;
    }
    
    protected override void OnReachDestination() {
        _targetFauna.Eat(_foodItem);
        Destroy(gameObject);
    }
}