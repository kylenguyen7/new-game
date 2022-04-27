

using UnityEngine;

public class ShopkeeperController : MonoBehaviour, IInteractable {
    [SerializeField] private string shopId;
    
    public void Interact() {
        MenuManager.Instance.Activate(shopId);
    }
}