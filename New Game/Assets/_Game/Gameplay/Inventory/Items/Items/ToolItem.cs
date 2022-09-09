using UnityEngine;

public abstract class ToolItem : Item {
    public abstract void onAction(GameObject target);
}