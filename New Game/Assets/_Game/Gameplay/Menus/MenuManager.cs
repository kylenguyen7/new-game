using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    [SerializeField] private List<MenuGroup> menuGroups;
    [SerializeField] private GameObject grayArea;
    
    public static MenuManager Instance;
    private MenuGroup _activeMenu;
    public bool HasActiveMenu => _activeMenu != null;

    public delegate void MenuEvent();

    public MenuEvent OnMenuOpenCallback;
    public MenuEvent OnMenuCloseCallback;

    public void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I) && _activeMenu == null) {
            Activate("inventory");
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _activeMenu != null) {
            Deactivate();
        }
    }

    public void Activate(String menuId) {
        if (HasActiveMenu) return;

        _activeMenu = GetMenuGroupById(menuId);
        _activeMenu.Activate();
        grayArea.SetActive(true);
        Time.timeScale = 0f;
        OnMenuOpenCallback?.Invoke();
    }

    public void Deactivate() {
        if (!HasActiveMenu) return;
        
        _activeMenu.Deactivate();
        grayArea.SetActive(false);
        _activeMenu = null;
        
        Time.timeScale = 1f;
        OnMenuCloseCallback?.Invoke();
    }

    private MenuGroup GetMenuGroupById(String id) {
        foreach(var group in menuGroups) {
            if (group.Name == id) {
                return group;
            }
        }
        
        Debug.LogError($"MenuManager could not find a menu with id {id}!");
        return null;
    }
    
    [Serializable]
    public class MenuGroup {
        public String Name;
        [SerializeField] private List<RectTransform> Menus;
        [SerializeField] private List<Vector2> Positions;
        [SerializeField] private Vector2 HotbarPosition;

        public void Activate() {
            for (int i = 0; i < Menus.Count; i++) {
                var menu = Menus[i];
                var position = Positions[i];
                
                menu.gameObject.SetActive(true);
                menu.anchoredPosition = position;
            }

            HotbarController.Instance.GetComponent<RectTransform>().anchoredPosition = HotbarPosition;
        }

        public void Deactivate() {
            foreach(var menu in Menus)
                menu.gameObject.SetActive(false);
            
            HotbarController.Instance.SetPosition(HotbarController.HotbarPosition.BOTTOM);
        }
    }
}
