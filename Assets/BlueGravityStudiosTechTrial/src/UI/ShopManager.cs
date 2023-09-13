using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    
    /// <summary>
    /// Data set containing all the outfits that can be bought
    /// </summary>
    public OutfitDataSet availableOutfits;
    
    /// <summary>
    /// Character inventory the Shop manager will notify when transactions are done
    /// </summary>
    public CharacterInventory characterInventory;
    
    /// <summary>
    /// UI the Shop manager will update upon equipping, buying or selling outfits
    /// </summary>
    public ShopUI shopUI;

    /// <summary>
    /// Called when player wants to start shopping
    /// </summary>
    /// <param name="i"></param>
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
    
    /// <summary>
    /// Tries to buy a product.
    /// </summary>
    /// <param name="productToBuy"></param>
    /// <returns></returns>
    public int TryBuy(ProductInfo productToBuy) {
        List<ProductInfo> productsToBuy = new List<ProductInfo>() { productToBuy };
        return TryBuy(productsToBuy);
    }
    
    /// <summary>
    /// Tries to sell a set of products
    /// </summary>
    /// <param name="productsToSell"></param>
    /// <returns></returns>
    public int TrySell(List<ProductInfo> productsToSell) {
        // Create transaction and notify the inventory
        float moneyToAdd = productsToSell.Sum(x => x.cost);
        characterInventory.ValidateTransaction(
            new InventoryTransaction(null,productsToSell, moneyToAdd));
        return InventoryTransaction.TRANSACTION_SUCCESS;
    }

    /// <summary>
    /// Tries to buy a set of products.
    /// </summary>
    /// <param name="productsToBuy"></param>
    /// <returns></returns>
    public int TryBuy(List<ProductInfo> productsToBuy) {
        // Create transaction and notify the inventory
        float moneyToReduce = productsToBuy.Sum(x => -x.cost);
        characterInventory.ValidateTransaction(
            new InventoryTransaction(productsToBuy, null, moneyToReduce));
        return InventoryTransaction.TRANSACTION_SUCCESS;
    }
}
