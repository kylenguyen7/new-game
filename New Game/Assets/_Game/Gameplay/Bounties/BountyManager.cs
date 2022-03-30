using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BountyManager : MonoBehaviour {
    public static BountyManager Instance;
    private List<Bounty> _bounties;
    [SerializeField] private TextMeshProUGUI _tmp;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _bounties = new List<Bounty>();
    }

    public void Update() {
        _tmp.text = GetBountyMessage();

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            _bounties.Add(new Bounty(EnemyType.SHOOTER, Random.Range(5, 10)));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            _bounties.Add(new Bounty(EnemyType.DASHER, Random.Range(5, 10)));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            _bounties.Add(new Bounty(EnemyType.MUSHROOM, Random.Range(5, 10)));
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            _bounties.Add(new Bounty(EnemyType.CHERRY, Random.Range(5, 10)));
        }
    }

    private String GetBountyMessage() {
        String result = "Bounties:";
        foreach (Bounty bounty in _bounties) {
            int startingCount = bounty.StartingCount;
            int count = bounty.Count;
            String type = EnemyTypes.EnemyTypeToString(bounty.Type);
            result += $"\nSlay {startingCount} {type}s. ({count} remaining!)";
        }

        return result;
    }

    public void ReceiveDeath(EnemyType type) {
        for (var i = 0; i < _bounties.Count; i++) {
            Bounty bounty = _bounties[i];
            if (bounty.Type == type) {
                bounty.Decrement();
                if (bounty.Count == 0) {
                    _bounties.Remove(bounty);
                    i--;
                }
            }
        }
    }

    public void AddBounty(Bounty bounty) {
        _bounties.Add(bounty);
    }
}

public class Bounty {
    private int _count;
    private int _startingCount;
    private EnemyType _type;
        
    public int Count => _count;
    public int StartingCount => _startingCount;
    public EnemyType Type => _type;

    public Bounty(EnemyType type, int count) {
        _type = type;
        _count = count;
        _startingCount = count;
    }
        
    public void Decrement() {
        _count = Math.Max(_count - 1, 0);
    }
}

public static class Bounties {
    public static Bounty GetRandomBounty() {
        float val = Random.value;
        if(val < 0.25f) return new Bounty(EnemyType.SHOOTER, Random.Range(5, 10));
        if(val < 0.50f) return new Bounty(EnemyType.DASHER, Random.Range(5, 10));
        if(val < 0.75f) return new Bounty(EnemyType.MUSHROOM, Random.Range(5, 10));
        return new Bounty(EnemyType.CHERRY, Random.Range(5, 10));
    }
}