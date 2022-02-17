
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

    [SerializeField] private float roomSpacing;
    [SerializeField] private GameObject dungeonPrefab;
    
    private void Start() {
        var dungeon = Dungeons.GenerateDungeon(size, roomsToGenerate, randomGiveUpChance);
        PlaceDungeon(dungeon);
        
        // TODO: remove
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void PlaceDungeon(RoomType[,] dungeon) {
        int width = dungeon.GetLength(0);
        int height = dungeon.GetLength(1);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                RoomType type = dungeon[x, y];
                if (type == RoomType.EMPTY) continue;

                float placeX = (x - width / 2) * roomSpacing;
                float placeY = (y - height / 2) * roomSpacing;

                Color color = Color.black;
                switch (type) {
                    case RoomType.BASIC:
                        color = Color.white;
                        break;
                    case RoomType.START:
                        color = Color.blue;
                        break;
                    case RoomType.END:
                        color = Color.red;
                        break;
                    case RoomType.BOSS :
                        color = Color.yellow;
                        break;
                }
                
                Instantiate(dungeonPrefab, new Vector2(placeX, placeY), Quaternion.identity)
                    .GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
