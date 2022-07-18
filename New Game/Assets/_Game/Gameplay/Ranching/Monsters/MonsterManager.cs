using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manager class for monsters (places, saves, and loads monsters)
 */
public class MonsterManager : Saveable {
    public static MonsterManager Instance;
    private Dictionary<String, NestController> _nestDictionary = new Dictionary<string, NestController>();
    private HashSet<MonsterController> _monsters = new HashSet<MonsterController>();
    
    // Override Saveable.Load to not load, since this is manually loaded b/c it relies on Nests being created already
    protected new void Start() { }

    public void ManualLoad() {
        Load();
    }
    
    // TODO: refactor save/load system with a proper priority mechanic
    protected override void Load() {
        foreach (SaveData.MonsterData monsterData in SaveData.Instance.RanchMonstersData.Monsters) {
            PlaceMonster(monsterData.SpeciesName, monsterData.NestId, Vector2.zero);
        }
    }

    public override void Save() {
        List<SaveData.MonsterData> monsters = new List<SaveData.MonsterData>();
        foreach (MonsterController monster in _monsters) {
            monsters.Add(new SaveData.MonsterData(monster.SpeciesName, monster.NestId));
        }

        SaveData.Instance.RanchMonstersData = new SaveData.MonstersData(monsters);
    }
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void RegisterNest(NestController nest) {
        Debug.Log($"Registering nest {nest.NestId}");
        _nestDictionary.Add(nest.NestId, nest);
    }

    public void PlaceMonster(String speciesName, String nestId, Vector2 position) {
        _nestDictionary.TryGetValue(nestId, out NestController nest);
        if (nest == null) {
            Debug.LogError($"Unable to find nest with nestId {nestId}.");
            return;
        }
        
        GameObject monsterPrefab = MonsterConstants.SpeciesNameToMonsterPrefab(speciesName);
        MonsterController monster = Instantiate(monsterPrefab).GetComponent<MonsterController>();

        if (position == Vector2.zero) {
            monster.transform.position = nest.transform.position;   // TODO: get random nest position
        } else {
            monster.transform.position = position;   // TODO: get random nest position
        }
        
        
        _monsters.Add(monster);
        monster.Init(nest);
    }
}