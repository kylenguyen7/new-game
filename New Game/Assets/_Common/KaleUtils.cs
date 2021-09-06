using UnityEngine;

namespace _Common {
    public class KaleUtils  {
        public static Vector2 GetMousePosWorldCoordinates() {
            Vector2 pos = Input.mousePosition;
            return Camera.main.ScreenToWorldPoint(pos);
        }
    }
}