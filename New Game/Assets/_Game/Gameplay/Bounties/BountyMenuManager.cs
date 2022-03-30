using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyMenuManager : MonoBehaviour {
    public static BountyMenuManager Instance;
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject bountyCardPrefab;
    [SerializeField] private Transform bountyCardParentInScene;
    [SerializeField] private int numDailyBounties;
    private bool menuActive;

    public void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        RandomizeBounties();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            DeactivateMenu();
        }
    }

    public void ActivateMenu() {
        shopMenu.SetActive(true);
        Time.timeScale = 0f;
        menuActive = true;
    }

    public void DeactivateMenu() {
        if (!menuActive) return;
        shopMenu.SetActive(false);
        Time.timeScale = 1f;
        menuActive = false;
    }

    /**
     * Clears bounties and generates new ones (for the end of the day).
     */
    public void RandomizeBounties() {
        for (int i = 0; i < bountyCardParentInScene.childCount; i++) {
            Destroy(bountyCardParentInScene.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < numDailyBounties; i++) {
            var bountyCard = Instantiate(bountyCardPrefab, bountyCardParentInScene).GetComponent<BountyCardManager>();
            bountyCard.Init(Bounties.GetRandomBounty());
        }
    }
}
