using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInventory : MonoBehaviour {
    
    /// <summary>
    /// Products owned by the player
    /// </summary>
    [Header("Inventory")]
    public List<ProductInfo> ownedProducts;

    /// <summary>
    /// Gold the player has.
    /// </summary>
    [SerializeField] private float goldAvailable;

    public float Gold => goldAvailable;
    
    [Header("Extra Components")]
    /// <summary>
    /// Outfit manager, which contains the outfits equipped by the player and the sprite libraries.
    /// </summary>
    [SerializeField] private OutfitManager outfitManager;

    [Header("UI elements")]
    /// <summary>
    /// UI that displays the inventory of the player
    /// </summary>
    [SerializeField] private CharacterInventoryUI inventoryUI;

    /// <summary>
    /// Obtains all the products of a type in the players inventory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> GetProductsOfType<T>() where T : ProductInfo {
        return ownedProducts.FindAll(x=>x.GetType()==typeof(T)).Cast<T>().ToList();
    }
    
    /// <summary>
    /// Obtains the outfit worn by the player
    /// </summary>
    /// <returns></returns>
    public List<OutfitInfo> GetWornOutfits() {
        return outfitManager.wornOutfits;
    }

    /// <summary>
    /// Called when a request to equip an outfit is issued.
    /// </summary>
    /// <param name="outfitInfo"></param>
    public void OnEquipOutfitRequest(OutfitInfo outfitInfo) {
        outfitManager.EquipOutfit(outfitInfo);
    }

    /// <summary>
    /// Executes a transaction. Adding objects and gold.
    /// </summary>
    /// <param name="transaction"></param>
    public void ValidateTransaction(InventoryTransaction transaction) {
        goldAvailable += transaction.goldToAdd;
        if (transaction.productsToAdd != null) {
            ownedProducts.AddRange(transaction.productsToAdd);
        }
        
        if (transaction.productsToRemove != null) {
            transaction.productsToRemove.ForEach(x=>ownedProducts.Remove(x));
        }
    }

    /// <summary>
    /// Displays the inventory UI. Also loads the items the player has.
    /// </summary>
    public void DisplayInventoryUI() {
        inventoryUI.DisplayUI();
    }
}