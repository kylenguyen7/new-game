using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEngine;

[CreateAssetMenu(order = 7, menuName = "Apothecary/MonsterItem")]
// TODO: refactor this into an abstract class like PlaceableItem that has abstract methods for CanPlace; refactor placement indicator to play nice with these changes
public class MonsterItem : Item {
    [SerializeField] private GameObject monsterPrefab;
    public Sprite Sprite => monsterPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
    
    public override void OnSelect() {
        if (MonsterManager.Instance != null) {
            MonsterPlacementIndicatorController.Instance.StartPlacing(this);
        }
    }

    public override void OnDeselect() {
        if (MonsterManager.Instance != null) {
            MonsterPlacementIndicatorController.Instance.StopPlacing();
        }
    }

    public Collider2D CanPlace(Vector2 position) {
        return Physics2D.OverlapPoint(position, LayerMask.GetMask("Nest"));
    }
    
    public override void OnClick() {
        Vector2 mousePos = KaleUtils.GetMousePosWorldCoordinates();
        Collider2D collider = CanPlace(mousePos);
        
        if (MonsterManager.Instance != null && collider != null) {
            NestController nest = collider.GetComponent<NestController>();
            MonsterManager.Instance.PlaceMonster(monsterPrefab.GetComponent<MonsterController>().SpeciesName, nest.NestId, mousePos);
            HotbarController.Instance.RemoveOneFromActiveSlot();
        }
    }
}
