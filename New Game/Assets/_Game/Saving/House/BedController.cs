using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedController : MonoBehaviour {
    private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;
    private bool _leftBed = false;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && _leftBed && CompleteOverlap(_collider, other)) {
            EndDay();
        }
    }

    private static void EndDay() {
        GlobalTime.Instance.Sleep();
        GlobalTime.Instance.Save();
        Inventory.Instance.Save();
        SaveData.Instance.ManualSave();
        TransitionHandler.Instance.SaveSceneAndLoadNewScene("HouseScene");
    }

    private void OnTriggerExit2D(Collider2D other) {
        _leftBed = true;
    }

    private static bool CompleteOverlap(Collider2D outer, Collider2D inner) {
        Bounds innerBounds = inner.bounds;
        Vector2 innerBottomLeftCoords = innerBounds.center - innerBounds.extents;
        Vector2 innerTopRightCoords = innerBounds.center + innerBounds.extents;

        Bounds outerBounds = outer.bounds;
        Vector2 outerBottomLeftCoords = outerBounds.center - outerBounds.extents;
        Vector2 outerTopRightCoords = outerBounds.center + outerBounds.extents;
        
        if (innerBottomLeftCoords.x < outerBottomLeftCoords.x || innerBottomLeftCoords.y < outerBottomLeftCoords.y) return false;
        if (innerTopRightCoords.x > outerTopRightCoords.x || innerTopRightCoords.y > outerTopRightCoords.y) return false;

        return true;
    }
}
