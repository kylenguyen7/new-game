
using System;
using System.Collections.Generic;
using _Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WorldObjectGrid : Saveable {
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject indicatorPrefab;
    
    [SerializeField] private List<GameObject> debrisPrefabs;
    [SerializeField] private float debrisSpawnChance;
    
    private WorldObjectController[,] _world;
    private IndicatorController[,] _indicators;

    private bool _placing;
    private WorldObjectController _currentPlacing;
    [SerializeField] private SpriteRenderer placingSprite;

    public static WorldObjectGrid Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // Create indicators across grid
        _world = new WorldObjectController[width, height];
        _indicators = new IndicatorController[width, height];
        
        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                var indicator = Instantiate(indicatorPrefab, transform);
                indicator.transform.localPosition = new Vector3(x, y, 0);
                _indicators[x, y] = indicator.GetComponent<IndicatorController>();
            }
        }
    }

    private void OnEnable() {
        // Subscribe to menu open event (when menu opens, stop placing)
        MenuManager.Instance.OnMenuOpenCallback += StopPlacing;
        MenuManager.Instance.OnMenuCloseCallback += ManualHotbarCheck;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable() {
        MenuManager.Instance.OnMenuOpenCallback -= StopPlacing;
        MenuManager.Instance.OnMenuCloseCallback -= ManualHotbarCheck;
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    
    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        ManualHotbarCheck();
    }

    private void ManualHotbarCheck() {
        var item = HotbarController.Instance.SelectedItem;
        if (item != null && item.Type == Item.ItemType.WORLD_OBJECT) {
            StartPlacing(HotbarController.Instance.SelectedItem);
        }
    }
    
    public void StartPlacing(Item item) {
        _currentPlacing = item.WorldObjectPrefab.GetComponent<WorldObjectController>();
        placingSprite.sprite = item.WorldObjectPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        _placing = true;
    }

    public void StopPlacing() {
        ClearIndicators();
        placingSprite.sprite = null;
        _placing = false;
    }

    private void Update() {
        if (_placing && MenuManager.Instance.HasActiveMenu) {
            StopPlacing();
        }
        
        if (!_placing) {
            return;
        }
        
        ClearIndicators();

        // Rounded position of cursor in world - (0, 0) is room center
        int mouseX = Mathf.RoundToInt(KaleUtils.GetMousePosWorldCoordinates().x);
        int mouseY = Mathf.RoundToInt(KaleUtils.GetMousePosWorldCoordinates().y);
        
        // Rounded position of origin of object in grid - (0, 0) is origin of the grid
        int gridX = mouseX - (int)transform.position.x + _currentPlacing.MinX;
        int gridY = mouseY - (int)transform.position.y + _currentPlacing.MinY;
        
        if (gridX + _currentPlacing.Width - 1 >= _world.GetLength(0)) {
            gridX = _world.GetLength(0) - _currentPlacing.Width;
        } else if (gridX < 0) {
            gridX = 0;
        }
                
        if (gridY + _currentPlacing.Height - 1 >= _world.GetLength(1)) {
            gridY = _world.GetLength(1) - _currentPlacing.Height;
        } else if (gridY < 0) {
            gridY = 0;
        }

        int objectOriginX = gridX - _currentPlacing.MinX;
        int objectOriginY = gridY - _currentPlacing.MinY;
        placingSprite.gameObject.transform.position = _indicators[objectOriginX, objectOriginY].transform.position;

        bool placementValid = true;
        foreach (var coord in _currentPlacing.Coords) {
            int x = objectOriginX + coord.Item1;
            int y = objectOriginY + coord.Item2;

            if (_world[x, y] == null) {
                _indicators[x, y].SetMode(IndicatorController.IndicatorMode.GREEN);
            } else {
                _indicators[x, y].SetMode(IndicatorController.IndicatorMode.RED);
                placementValid = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && placementValid) {
            PlaceWorldObject(_currentPlacing, objectOriginX, objectOriginY, "");
            
            // Tools don't disappear, e.g. hoe
            if(HotbarController.Instance.SelectedItem.Type != Item.ItemType.TOOL) 
                HotbarController.Instance.RemoveOneFromActiveSlot();
        }
    }
    
    private void ClearIndicators() {
        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                _indicators[x, y].SetMode(IndicatorController.IndicatorMode.OFF);
            }
        }
    }

    public void PrintGrid() {
        String gridString = "";
        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                if (_world[x, y] == null) {
                    gridString += "null ";
                } else {
                    gridString += _world[x, y].Id + " ";
                }
                
            }
            gridString += "\n";
        }
        Debug.Log(gridString);
    }
    
    private void OnDrawGizmos() {
        float centerX = transform.position.x + width / 2f - 0.5f;
        float centerY = transform.position.y + height / 2f - 0.5f;
        Gizmos.DrawWireCube(new Vector3(centerX, centerY, 0), new Vector3(width, height, 1));
    }

    public void PlaceWorldObjectWorldCoords(WorldObjectController worldObjectController, float worldX, float worldY,
        String metadata) {
        PlaceWorldObject(worldObjectController, 
            Mathf.FloorToInt(worldX - transform.position.x), 
            Mathf.FloorToInt(worldY - transform.position.y),
            metadata);
    }

    public void PlaceWorldObject(WorldObjectController worldObjectController, int x, int y, String metadata) {
        Vector3 worldPosition = _indicators[x, y].transform.position;
        var controller = Instantiate(worldObjectController, worldPosition, Quaternion.identity)
            .GetComponent<WorldObjectController>();
        controller.SetOrigin(x, y);

        if (metadata != "") {
            controller.LoadMetaData(metadata);
        }
        
        foreach(var coord in controller.Coords) {
            int coordX = x + coord.Item1;
            int coordY = y + coord.Item2;
            _world[coordX, coordY] = controller;
        }
    }

    public void RemoveWorldObject(WorldObjectController worldObjectController) {
        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                if (_world[x, y] == worldObjectController) {
                    _world[x, y] = null;
                }
            }
        }
    }

    public override void Save() {
        List<SaveData.WorldObjectData> worldObjects = new List<SaveData.WorldObjectData>();
        HashSet<WorldObjectController> added = new HashSet<WorldObjectController>();

        foreach (WorldObjectController worldObject in _world) {
            if (worldObject == null || added.Contains(worldObject)) continue;
            worldObjects.Add(new SaveData.WorldObjectData(worldObject.Id, worldObject.OriginX, worldObject.OriginY, worldObject.GetMetaData()));
            added.Add(worldObject);
        }
        SaveData.Instance.SavedTownWorldObjectsData = new SaveData.WorldObjectsData(worldObjects, true);
    }
    
    protected override void Load() {
        var data = SaveData.Instance.SavedTownWorldObjectsData;
        var worldObjects = data.WorldObjects;
        foreach (var worldObjectData in worldObjects) {
            var item = ItemConstants.ItemIdToScriptableObject(worldObjectData.Id);
            var prefab = item.WorldObjectPrefab;
            PlaceWorldObject(prefab.GetComponent<WorldObjectController>(), worldObjectData.X, worldObjectData.Y, worldObjectData.Metadata);
        }

        // Generate debris
        Debug.Log($"Loaded world object grid data with generatedDebris: {data.GeneratedDebris}");
        if (!data.GeneratedDebris) {
            GenerateDebris();
        }
        
        // Load monsters (has to be done after nests are created)
        if (MonsterManager.Instance != null) {
            MonsterManager.Instance.ManualLoad();
        }
    }

    private void GenerateDebris() {
        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                if (_world[x, y] == null && Random.value < debrisSpawnChance) {
                    int index = Random.Range(0, debrisPrefabs.Count);
                    var worldObjectController = debrisPrefabs[index].GetComponent<WorldObjectController>();
                    PlaceWorldObject(worldObjectController, x, y, "");
                }
            }
        }
    }
}