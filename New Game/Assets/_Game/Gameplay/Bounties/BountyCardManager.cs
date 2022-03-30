using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BountyCardManager : MonoBehaviour {
    [SerializeField] private EnemyInfoScriptableObject[] enemyInfo;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _tmp;
    
    private Bounty _bounty;
    
    public void OnClick() {
        BountyManager.Instance.AddBounty(_bounty);
        Destroy(gameObject);
    }

    public void Init(Bounty bounty) {
        _bounty = bounty;
        
        foreach (var info in enemyInfo) {
            if (info.Type == bounty.Type) {
                _image.sprite = info.Sprite;
                _tmp.text = $"Slay {bounty.StartingCount} {info.Name}.";
                return;
            }
        }
        
        _tmp.text = $"Could not find enemy of type {bounty.Type}. Check BountyCardManager.enemyInfo";
    }
}
