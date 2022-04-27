
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Class responsible for placing the dungeon, which has been created via
 * the Dungeons.GenerateDungeon class.
 */
public class DungeonProceduralGenerator : MonoBehaviour {
    [SerializeField] private int size;
    [SerializeField] private int roomsToGenerate;
    [SerializeField] private float randomGiveUpChance;
    [SerializeField] private float roomWidth;
    [SerializeField] private float roomHeight;
    [SerializeField] private List<GameObject> dungeonPrefabs;

    private static RoomType[,] dungeon;

    public static RoomType GetDungeonRoomType(int x, int y) {
        if (!Dungeons.InBounds(dungeon, x, y)) return RoomType.NULL;
        return dungeon[x, y];
    }

    public static bool HasRoomAt(int x, int y) {
        if (!Dungeons.InBounds(dungeon, x, y)) return false;
        return dungeon[x, y] != RoomType.EMPTY;
    }
    
    private static RoomBrain[,] dungeonBrains;

    public static RoomBrain GetDungeonRoomBrain(int x, int y) {
        if (!Dungeons.InBounds(dungeonBrains, x, y)) return null;
        return dungeonBrains[x, y];
    }

    private static RoomBrain currentBrain;

    public static RoomBrain GetCurrentBrain() {
        return currentBrain;
    }

    public static void SetCurrentBrain(int x, int y) {
        currentBrain = dungeonBrains[x, y];
    }

    private void Awake() {
        dungeon = Dungeons.GenerateDungeon(size, roomsToGenerate, randomGiveUpChance);
        dungeonBrains = new RoomBrain[dungeon.GetLength(0), dungeon.GetLength(1)];
        PlaceDungeon(dungeon);
    }

    private void Update() {
        currentBrain.Tick();
        
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Dungeons.PrintDungeon(dungeon);
        }
    }

    private void PlaceDungeon(RoomType[,] dungeon) {
        int width = dungeon.GetLength(0);
        int height = dungeon.GetLength(1);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                RoomType type = dungeon[x, y];
                if (type == RoomType.EMPTY) continue;

                float placeX = (x - width / 2) * roomWidth;
                float placeY = (y - height / 2) * roomHeight;

                var dungeonPrefab = dungeonPrefabs[Random.Range(0, dungeonPrefabs.Count)];
                var brain = Instantiate(dungeonPrefab, new Vector3(placeX, placeY, 0), Quaternion.identity).GetComponent<RoomBrain>();
                brain.Init(x, y);
                if (type == RoomType.START) {
                    currentBrain = brain;
                }

                dungeonBrains[x, y] = brain;
            }
        }
    }
}