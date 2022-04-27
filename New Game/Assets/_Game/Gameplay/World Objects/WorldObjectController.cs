

using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Editor;

public class WorldObjectController : MonoBehaviour {
    // String literal representing coordinates, e.g. "0 0 1 0 0 1"
    [SerializeField] private String coordinates;
    [SerializeField] private Item itemScriptableObject;
    [SerializeField] private GameObject itemPrefab;
    private bool _hovered;

    public String Id => itemScriptableObject.Id;

    public virtual String GetMetaData() {
        return "";
    }
    
    public virtual void LoadMetaData(String data) {
        return;
    }

    public void OnMouseOver() {
        if (itemScriptableObject != null && Input.GetMouseButtonDown(2)) {
            DestroyMe();
        }
    }

    private void DestroyMe() {
        var item = Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponent<ItemController>();
        item.Init(itemScriptableObject);
        WorldObjectGrid.Instance.RemoveWorldObject(this);
        Destroy(gameObject);
    }

    private Tuple<int, int>[] _coords;
    public Tuple<int, int>[] Coords {
        get {
            if (_coords == null) {
                _coords = CoordsStringToList(coordinates);
            }
            return _coords;
        }
    }

    public Tuple<int, int> OriginInRoom { get; private set; }

    public int OriginX => OriginInRoom.Item1;
    public int OriginY => OriginInRoom.Item2;

    private int _width;
    public int Width {
        get {
            if (_width == 0) {
                _width = GetWidth(Coords);
            }
            return _width;
        }
    }

    private int _height;

    public int Height {
        get {
            if (_height == 0) {
                _height = GetHeight(Coords);
            }
            return _height;
        }
    }

    private int _minX = 1;
    public int MinX {
        get {
            if (_minX == 1) {
                _minX = GetMinX(Coords);
            }
            return _minX;
        }
    }

    private int _minY = 1;
    public int MinY {
        get {
            if (_minY == 1) {
                _minY = GetMinY(Coords);
            }
            return _minY;
        }
    }
    

    public void SetOrigin(int x, int y) {
        OriginInRoom = new Tuple<int, int>(x, y);
    }

    private static Tuple<int, int>[] CoordsStringToList(String coordsString) {
        string[] coords = coordsString.Split(' ');
        Tuple<int, int>[] result = new Tuple<int, int>[coords.Length / 2];
        
        for (int i = 0; i < result.Length; i++) {
            int x = Int32.Parse(coords[2 * i]);
            int y = Int32.Parse(coords[2 * i + 1]);

            result[i] = new Tuple<int, int>(x, y);
        }
        return result;
    }

    private static int GetWidth(Tuple<int, int>[] coords) {
        int max = 0;
        int min = 100;
        foreach (var coord in coords) {
            max = Mathf.Max(max, coord.Item1);
            min = Mathf.Min(min, coord.Item1);
        }
        return (max - min) + 1;
    }
    
    private static int GetHeight(Tuple<int, int>[] coords) {
        int max = 0;
        int min = 100;
        foreach (var coord in coords) {
            max = Mathf.Max(max, coord.Item2);
            min = Mathf.Min(min, coord.Item2);
        }
        return (max - min) + 1;
    }
    
    private static int GetMinX(Tuple<int, int>[] coords) {
        int min = 1;
        foreach (var coord in coords) {
            min = Mathf.Min(min, coord.Item1);
        }

        return min;
    }
    
    private static int GetMinY(Tuple<int, int>[] coords) {
        int min = 1;
        foreach (var coord in coords) {
            min = Mathf.Min(min, coord.Item2);
        }

        return min;
    }
}