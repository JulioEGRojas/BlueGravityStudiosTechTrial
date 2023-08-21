using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInventoryUI : MonoBehaviour {

    /// <summary>
    /// Container for the items the player has.
    /// </summary>
    [SerializeField] private OutfitItemsUI ownedItemsUI;

    /// <summary>
    /// Text that displays the gold the player has. 
    /// </summary>
    [SerializeField] private TextMeshProUGUI goldText;

    /// <summary>
    /// Event called when a display inventory request is issued
    /// </summary>
    [SerializeField] private UnityEvent OnInventoryDisplayRequest;

    /// <summary>
    /// Event called when a hide inventory request is issued
    /// </summary>
    [SerializeField] private UnityEvent OnInventoryHideRequest;

    /// <summary>
    /// Inventory of the player. Notifies it when the player uses or equips an item by using the UI.
    /// </summary>
    [SerializeField] private CharacterInventory characterInventory;
    
    private void Awake() {
        ownedItemsUI.Initialize();
    }

    /// <summary>
    /// Displays gold and items on the Inventory UI.
    /// </summary>
    /// <param name="characterInventory"></param>
    public void LoadInventoryInfo(CharacterInventory characterInventory) {
        goldText.text = "" + characterInventory.Gold;
        PopulateItemsUIWith(characterInventory.GetProductsOfType<OutfitInfo>());
    }

    /// <summary>
    /// Populates the inventory UI with the given items
    /// </summary>
    /// <param name="outfitInfo"></param>
    public void PopulateItemsUIWith(List<OutfitInfo> outfitInfo) {
        ownedItemsUI.LoadInfo(outfitInfo);
    }

    /// <summary>
    /// Called when an outfit container is clicked
    /// </summary>
    /// <param name="outfitContainerUI"></param>
    public void OnOutfitEquipRequest(OutfitContainerUI outfitContainerUI) {
        characterInventory.OnEquipOutfitRequest(outfitContainerUI.productInfo);
    }

    /// <summary>
    /// Displays the inventory UI.
    /// </summary>
    public void DisplayUI() {
        LoadInventoryInfo(characterInventory);
        OnInventoryDisplayRequest.Invoke();
    }
    
    /// <summary>
    /// Hides the inventory UI.
    /// </summary>
    public void HideUI() {
        OnInventoryHideRequest.Invoke();
    }
}
