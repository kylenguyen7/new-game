using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaunaController : WorldObjectController {
    [SerializeField] private Animator sNsAnimator;
    [SerializeField] private ReactionController reactionController;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] private FaunaInfo myInfo;
    [SerializeField] private GameObject ghostPrefab;
    
    private int _xp;
    private int _xpGain;            // xp that has been fed but not added to _xp until the next day
    private int _lastDateFed;
    public int Xp => _xp;
    public int XpGain => _xpGain;
    public int LastDateFed => _lastDateFed;

    private bool FedToday => LastDateFed == GlobalTime.Instance.CurrentDateTime.Date;
    public bool IsMature { get; private set; }

    private void Awake() {
        Init(0, 0, GlobalTime.Instance.CurrentDateTime.Date - 1);
    }

    public override String GetMetaData() {
        return $"{Xp} {XpGain} {LastDateFed} {IsMature}";
    }
    
    public override void LoadMetaData(String data) {
        string[] tokens = data.Split(' ');
        
        int xp = Int32.Parse(tokens[0]);
        int xpGain = Int32.Parse(tokens[1]);
        int lastDateFed = Int32.Parse(tokens[2]);
        bool isMature = Boolean.Parse(tokens[3]);
        

        // Fauna is marked as dead, or hasn't been fed in three days
        if (GlobalTime.Instance.CurrentDateTime.Date - lastDateFed >= 3 && !isMature) {
            Instantiate(ghostPrefab, transform.position, Quaternion.identity).GetComponent<GhostController>();
            Destroy(gameObject);
            return;
        }
        
        // Fauna is alive and chillin'
        if (lastDateFed < GlobalTime.Instance.CurrentDateTime.Date) {
            Init( xp + xpGain, 0, lastDateFed);
        } else {
            Init(xp, xpGain, lastDateFed);
        }
    }
    

    public void Init(int xp, int xpGain, int lastDateFed) {
        _lastDateFed = lastDateFed;
        _xpGain = xpGain;
        _xp = xp;

        int stage = GetStage(xp);
        sprite.sprite = myInfo.Stages[stage];
        IsMature = stage == myInfo.XpStages.Length - 1;
        
        if (IsMature) {
            reactionController.React(Reaction.MONEY, false);
        } else if (!FedToday) {
            reactionController.React(Reaction.SAD, false);
        }
    }

    private int GetStage(int xp) {
        for (int i = myInfo.XpStages.Length - 1; i >= 0; i--) {
            int stageXp = myInfo.XpStages[i];
            if (xp >= stageXp) {
                return i;
            }
        }
        return -1;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1) && IsMature) {
            Gold.Instance.Quantity += 100;
            Destroy(gameObject);
        }
    }

    private void OnMouseDown() {
        if (IsMature || FedToday) return;

        var item = HotbarController.Instance.SelectedItem;
        if (item != null && item.Type == Item.ItemType.TINCTURE) {

            bool found = false;
            for (int i = 0; i < myInfo.FoodList.Length; i++) {
                if (myInfo.FoodList[i] == item) {
                    found = true;
                    break;
                }
            }

            if (found) {
                Item food = HotbarController.Instance.RemoveOneFromActiveSlot();
                FoodSourceController.Instance.Feed(this, food);
                _lastDateFed = GlobalTime.Instance.CurrentDateTime.Date;
            }
            else {
                Debug.LogWarning($"{gameObject.name} doesn't eat {item.Id}!!");
            }
            
        }
    }

    public void Eat(Item foodItem) {
        sNsAnimator.SetTrigger("squash");
        reactionController.React(Reaction.HEART, true);
        
        _xpGain = 0;
        for (int i = 0; i < myInfo.FoodList.Length; i++) {
            if (myInfo.FoodList[i] == foodItem) {
                _xpGain = myInfo.FoodXpValues[i];
                break;
            }
        }

        if (_xpGain == 0) {
            Debug.LogWarning($"No xp value for food with id {foodItem.Id} found for enemy {gameObject.name}! Gaining +1 xp.");
            _xpGain = 1;
        }
        
        Debug.Log($"{gameObject.name} ate a {foodItem.Id}");
    }
}
