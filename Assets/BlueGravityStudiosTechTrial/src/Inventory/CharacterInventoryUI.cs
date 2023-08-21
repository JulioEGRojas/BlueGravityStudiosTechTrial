using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInventoryUI : MonoBehaviour {

    [SerializeField] private OutfitItemsUI ownedItemsUI;

    [SerializeField] private TextMeshProUGUI goldText;

    [SerializeField] private UnityEvent OnInventoryDisplayRequest;

    [SerializeField] private UnityEvent OnInventoryHideRequest;

    [SerializeField] private CharacterInventory characterInventory;
    
    private void Awake() {
        ownedItemsUI.Initialize();
    }

    public void LoadInventoryInfo(CharacterInventory characterInventory) {
        goldText.text = "" + characterInventory.Gold;
        PopulateItemsUIWith(characterInventory.GetProductsOfType<OutfitInfo>());
    }

    public void PopulateItemsUIWith(List<OutfitInfo> outfitInfo) {
        ownedItemsUI.LoadInfo(outfitInfo);
    }

    public void OnOutfitEquipRequest(OutfitContainerUI outfitContainerUI) {
        characterInventory.OnEquipOutfitRequest(outfitContainerUI.productInfo);
    }

    public void DisplayUI() {
        OnInventoryDisplayRequest.Invoke();
    }
    
    public void HideUI() {
        OnInventoryHideRequest.Invoke();
    }
}
