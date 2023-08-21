using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    
    /// <summary>
    /// Data set containing all the outfits that can be bought
    /// </summary>
    public OutfitDataSet availableOutfits;
    
    public CharacterInventory characterInventory;
    
    public ShopUI shopUI;

    public void OnShopRequest(Interactor i) {
        shopUI.StartShopping(i);
    }

    public List<OutfitInfo> GetOwnedOutfits() {
        return characterInventory.GetProductsOfType<OutfitInfo>();
    }

    public List<OutfitInfo> GetWornOutfits() {
        return characterInventory.GetWornOutfits();
    }
    
    /// <summary>
    /// Gets the outfits the player doesn't own yet.
    /// </summary>
    /// <returns></returns>
    public List<OutfitInfo> GetNonOwnedOutfits() {
        return availableOutfits.items.FindAll(x => !characterInventory.ownedProducts.Contains(x));
    }

    public float GetPlayerMoney() {
        return characterInventory.Gold;
    }
    
    public int TryBuy(ProductInfo productToBuy) {
        List<ProductInfo> productsToBuy = new List<ProductInfo>() { productToBuy };
        return TryBuy(productsToBuy);
    }
    
    public int TrySell(List<ProductInfo> productsToSell) {
        float moneyToAdd = productsToSell.Sum(x => x.cost);
        characterInventory.ValidateTransaction(
            new InventoryTransaction(null,productsToSell, moneyToAdd));
        return InventoryTransaction.TRANSACTION_SUCCESS;
    }

    public int TryBuy(List<ProductInfo> productsToBuy) {
        float moneyToReduce = productsToBuy.Sum(x => -x.cost);
        characterInventory.ValidateTransaction(
            new InventoryTransaction(productsToBuy, null, moneyToReduce));
        return InventoryTransaction.TRANSACTION_SUCCESS;
    }
}
