using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DateTime = GlobalTime.DateTime;

public class SaveData : MonoBehaviour {
    public static SaveData Instance;

    // Serialized fields
    [SerializeField] private GlobalTimeData globalTimeData;

    public GlobalTimeData SavedGlobalTimeData {
        get => globalTimeData;
        set => globalTimeData = value;
    }

    // [SerializeField] private List<FactoryData> factories = new List<FactoryData>();
    //
    // public List<FactoryData> Factories {
    //     get => factories;
    //     set => factories = value;
    // }
    // [Serializable]
    // public struct FactoryData {
    //     [SerializeField] private Vector2 location;
    //     public Vector2 Location => location;
    //     
    //     [SerializeField] private FactoryController.Status status;
    //     public FactoryController.Status Status => status;
    //     
    //     [SerializeField] private DateTime startTime;
    //     public DateTime StartTime => startTime;
    //
    //     [SerializeField] private FactoryConstants.FactoryType factoryType;
    //     public FactoryConstants.FactoryType FactoryType => factoryType;
    //     
    //     public FactoryData(Vector2 location, FactoryController.Status status, DateTime startTime, FactoryConstants.FactoryType factoryType) {
    //         this.location = location;
    //         this.status = status;
    //         this.startTime = startTime;
    //         this.factoryType = factoryType;
    //     }
    // }
    
    [SerializeField] private List<BitletData> townBitletData;

    public List<BitletData> TownBitletData {
        get => townBitletData;
        set => townBitletData = value;
    }

    [Serializable]
    public struct BitletRoomData {
        [SerializeField] private List<BitletData> bitlets;
        public List<BitletData> Bitlets => bitlets;
        
        public BitletRoomData(List<BitletData> bitlets) {
            this.bitlets = bitlets;
        }
    }

    [Serializable]
    public struct BitletData {
        [SerializeField] private BitletType type;
        public BitletType Type => type;
        
        [SerializeField] private String name;
        public String Name => name;
        
        [SerializeField] private float x;
        public float X => x;
        [SerializeField] private float y;
        public float Y => y;

        [SerializeField] private int treatmentProgress;
        public int TreatmentProgress => treatmentProgress;

        [SerializeField] private int lastDayTreated;
        public int LastDayTreated => lastDayTreated;
        
        public BitletData(BitletType type, String name, float x, float y, int treatmentProgress, int lastDayTreated) {
            this.type = type;
            this.name = name;
            this.x = x;
            this.y = y;
            this.treatmentProgress = treatmentProgress;
            this.lastDayTreated = lastDayTreated;
        }
    }

    [Serializable]
    public struct GlobalTimeData {
        [SerializeField] private int date;
        public int Date => date;
        public GlobalTimeData(int date) {
            this.date = date;
        }
    }
    
    [SerializeField] private InventoryData inventoryData;

    public InventoryData SavedInventoryData {
        get => inventoryData;
        set => inventoryData = value;
    }

    [Serializable]
    public struct InventoryData {
        [SerializeField] private List<SlotData> slotData;
        public List<SlotData> SlotData => slotData;
        
        public InventoryData(List<SlotData> slotData) {
            this.slotData = slotData;
        }
    }

    [Serializable]
    public struct SlotData {
        [SerializeField] private String itemId;
        public String Id => itemId;
        [SerializeField] private int quantity;
        public int Quantity => quantity;
        
        public SlotData(String itemId, int quantity) {
            this.itemId = itemId;
            this.quantity = quantity;
        }
    }
    
    [SerializeField] private WorldObjectsData townWorldObjectsData;

    public WorldObjectsData SavedTownWorldObjectsData {
        get => townWorldObjectsData;
        set => townWorldObjectsData = value;
    }

    [Serializable]
    public struct WorldObjectsData {
        [SerializeField] private List<WorldObjectData> worldObjects;
        [SerializeField] private bool generatedDebris;
        public List<WorldObjectData> WorldObjects => worldObjects;
        public bool GeneratedDebris => generatedDebris;
        
        public WorldObjectsData(List<WorldObjectData> worldObjects, bool generatedDebris) {
            this.worldObjects = worldObjects;
            this.generatedDebris = generatedDebris;
        }
    }

    [Serializable]
    public struct WorldObjectData {
        [SerializeField] private String worldObjectItemId;
        public String Id => worldObjectItemId;
        [SerializeField] private int x;
        public int X => x;
        [SerializeField] private int y;
        public int Y => y;

        [SerializeField] private String metadata;
        public String Metadata => metadata;
        
        public WorldObjectData(String worldObjectItemId, int x, int y, String metadata) {
            this.worldObjectItemId = worldObjectItemId;
            this.x = x;
            this.y = y;
            this.metadata = metadata;
        }
    }
    
    [SerializeField] private int gold;

    public int Gold {
        get => gold;
        set => gold = value;
    }

    [Serializable]
    public struct MonstersData {
        [SerializeField] private List<MonsterData> monsters;
        public List<MonsterData> Monsters => monsters;

        public MonstersData(List<MonsterData> monsters) {
            this.monsters = monsters;
        }
    }

    [Serializable]
    public struct MonsterData {
        [SerializeField] private String speciesName;
        [SerializeField] private String nestId;

        public String SpeciesName => speciesName;
        public String NestId => nestId;

        public MonsterData(String speciesName, String nestId) {
            this.speciesName = speciesName;
            this.nestId = nestId;
        }
    }

    [SerializeField] private MonstersData ranchMonstersData;
    public MonstersData RanchMonstersData {
        get => ranchMonstersData;
        set => ranchMonstersData = value;
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Load data from file once at application start.
        string json = KaleSaveSystem.Load();
        if(json != null) JsonUtility.FromJsonOverwrite(json, this);
    }

    /**
     * Writes save to file. Called when the player goes to sleep.
     */
    public void ManualSave() {
        KaleSaveSystem.Save(JsonUtility.ToJson(this));
    }
}