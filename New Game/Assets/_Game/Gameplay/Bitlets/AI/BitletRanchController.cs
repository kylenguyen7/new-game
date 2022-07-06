using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class BitletRanchController : BitletController {
    // CONSTANT VARS
    [SerializeField] private Animator animator;
    [SerializeField] private String ailmentDesc;
    [SerializeField] private BitletType type;
    [SerializeField] private ReactionController reactionController;
    [SerializeField] private Reaction reaction;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Item radletItem;
    public BitletType BitletType => type;
    public override Animator Animator => animator;

    // RUNTIME VARS
    private String _name;
    public String Name => _name;
    private int _treatmentProgress;
    public int TreatmentProgress => _treatmentProgress;
    
    private int _lastDayTreated;
    public int LastDayTreated => _lastDayTreated;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            TooltipController.Instance.Title = $"{Name} (Bitlet)";
            TooltipController.Instance.Subtitle = ailmentDesc;
            TooltipController.Instance.Subtitle += $"\n{TreatmentProgress}/7 days to cure";
        }
    }

    private void OnMouseExit() {
        TooltipController.Instance.Title = "";
        TooltipController.Instance.Subtitle = "";
    }
    
    private void OnMouseDown() {
        if (TreatmentProgress == 6) {
            SpawnRadlets(10);
            Destroy(gameObject);
        } else if (LastDayTreated != GlobalTime.Instance.CurrentDateTime.Date && HotbarController.Instance.SelectedItem.Type == Item.ItemType.TINCTURE) {
            HotbarController.Instance.RemoveOneFromActiveSlot();
            UpdateTreatment(_treatmentProgress + 1, GlobalTime.Date);
            SpawnRadlets(Random.Range(2, 3));
        }
    }

    public void Init(String name, int treatmentProgress, int lastDayTreated) {
        _name = name;
        UpdateTreatment(treatmentProgress, lastDayTreated);
    }

    private void UpdateTreatment(int treatmentProgress, int lastDayTreated) {
        _treatmentProgress = treatmentProgress;
        _lastDayTreated = lastDayTreated;

        if (treatmentProgress == 6) {
            animator.runtimeAnimatorController = BitletConstants.BitletTypeToAnimator(BitletType.HAPPY);
            reactionController.React(Reaction.MONEY, false);
        } else if (GlobalTime.Date == LastDayTreated) {
            animator.runtimeAnimatorController = BitletConstants.BitletTypeToAnimator(type);
            reactionController.ClearReact();
        } else {
            animator.runtimeAnimatorController = BitletConstants.BitletTypeToAnimator(type);
            reactionController.React(reaction, false);
        }
    }
    
    private void SpawnRadlets(int numRadlets) {
        for (int i = 0; i < numRadlets; i++) {
            var item = Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponent<ItemController>();
            item.Init(radletItem);
        }
    }
}