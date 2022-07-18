

using System;
using _Common;
using UnityEngine;

/**
 * Indicator for placing monsters. Only allows placement of monsters inside nests.
 *
 * TODO: generalize this indicator to allow placement of all kinds of objects, each with their own placement criteria
 */
public class MonsterPlacementIndicatorController : MonoBehaviour {
    public static MonsterPlacementIndicatorController Instance;
    
    private MonsterItem _currentPlacing;
    [SerializeField] private SpriteRenderer indicator;
    [SerializeField] private SpriteRenderer validator;
    [SerializeField] private Sprite checkSprite;
    [SerializeField] private Sprite xSprite;

    public void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartPlacing(MonsterItem monsterItem) {
        _currentPlacing = monsterItem;
        indicator.sprite = monsterItem.Sprite;
    }

    public void StopPlacing() {
        _currentPlacing = null;
    }

    private void Update() {
        if (_currentPlacing != null) {
            transform.position = KaleUtils.GetMousePosWorldCoordinates();

            if (_currentPlacing.CanPlace(KaleUtils.GetMousePosWorldCoordinates())) {
                validator.sprite = checkSprite;
            } else {
                validator.sprite = xSprite;
            }
        } else {
            if (indicator.sprite != null) {
                indicator.sprite = null;
            }

            if (validator.sprite != null) {
                validator.sprite = null;
            }
        }
    }
}