using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInventory : MonoBehaviour {

    [SerializeField] private float goldAvailable;
    
    [SerializeField] private CharacterInventoryUI inventoryUI;

    public float Gold => goldAvailable;
    
    public List<ProductInfo> ownedProducts;
    
    [SerializeField] private OutfitManager outfitManager;

    public List<T> GetProductsOfType<T>() where T : ProductInfo {
        return ownedProducts.FindAll(x=>x.GetType()==typeof(T)).Cast<T>().ToList();
    }
    
    public List<OutfitInfo> GetWornOutfits() {
        return outfitManager.wornOutfits;
    }

    public void OnEquipOutfitRequest(OutfitInfo outfitInfo) {
        outfitManager.EquipOutfit(outfitInfo);
    }

    public void ValidateTransaction(InventoryTransaction transaction) {
        goldAvailable += transaction.goldToAdd;
        if (transaction.productsToAdd != null) {
            ownedProducts.AddRange(transaction.productsToAdd);
        }
        
        if (transaction.productsToRemove != null) {
            transaction.productsToRemove.ForEach(x=>ownedProducts.Remove(x));
        }
    }

    public void DisplayInventoryUI() {
        inventoryUI.DisplayUI();
        inventoryUI.LoadInventoryInfo(this);
    }
    
    public void HideInventoryUI() {
        inventoryUI.HideUI();
    }
}