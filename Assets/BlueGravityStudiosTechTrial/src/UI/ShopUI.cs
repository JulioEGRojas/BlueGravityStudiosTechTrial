using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {
    
    [Header("Buy Mode")]
    [SerializeField] private OutfitItemsUI buyModeOutfitItemsUI;

    [SerializeField] private OutfitContainerUI itemToBuyDisplay;
    
    [SerializeField] private TextMeshProUGUI buyModePlayerMoneyText;

    [SerializeField] private TextMeshProUGUI buyTransactionTotalCostText;

    [SerializeField] private TextMeshProUGUI notEnoughMoneyNotification;

    [SerializeField] private Button buyButton;
    
    [SerializeField] private Button buyAndEquipButton;
    
    [Header("Sell Mode")]
    [SerializeField] private OutfitItemsUI outfitsOwnedByPlayerItemsUI;

    [SerializeField] private OutfitItemsUI outfitsToBeSoldByPlayerItemsUI;
    
    [SerializeField] private TextMeshProUGUI sellModePlayerMoneyText;
    
    [SerializeField] private TextMeshProUGUI sellTransactionEarningsText;

    [SerializeField] private Button sellButton;
    
    /// <summary>
    /// Outfits the player has selected to sell
    /// </summary>
    private List<OutfitInfo> _outfitsToBeSoldByPlayer;

    [Header("References")]
    [SerializeField] private ShopManager shopManager;
    
    [Header("Events")]
    [SerializeField] private UnityEvent OnTransactionStarted;

    [SerializeField] private UnityEvent OnBuyModeActivate;
    
    [SerializeField] private UnityEvent OnSellModeActivate;
    
    [SerializeField] private UnityEvent OnTransactionCompleted;

    private void Awake() {
        buyModeOutfitItemsUI.Initialize();
        outfitsOwnedByPlayerItemsUI.Initialize();
        _outfitsToBeSoldByPlayer = new List<OutfitInfo>();
    }

    public void StartShopping(Interactor i) {
        // Default mode is buy mode
        OnTransactionStarted.Invoke();
        ActivateBuyMode();
    }
    
    public void ActivateBuyMode() {
        // Calculate the outfits the player can actually buy, and then tell the outfit UI to use that
        buyButton.interactable = false;
        buyAndEquipButton.interactable = false;
        itemToBuyDisplay.gameObject.SetActive(false);
        buyTransactionTotalCostText.text = "";
        List<OutfitInfo> itemsAvailableToBuy = shopManager.GetNonOwnedOutfits();
        buyModeOutfitItemsUI.LoadInfo(itemsAvailableToBuy);
        UpdatePlayerMoney();
        OnBuyModeActivate.Invoke();
    }

    public void ActivateSellMode() {
        // Clear the outfits being sold by the player
        _outfitsToBeSoldByPlayer.Clear();
        outfitsToBeSoldByPlayerItemsUI.ClearInfo();
        // Get the outfits the player has
        List<OutfitInfo> itemsOwnedByPlayer = shopManager.GetOwnedOutfits();
        outfitsOwnedByPlayerItemsUI.LoadInfo(itemsOwnedByPlayer);
        sellTransactionEarningsText.text = "";
        // After loading the info, make outfit containers that have an outfit worn by the player not interactable 
        List<OutfitInfo> outfitsWornByPlayer = shopManager.GetWornOutfits();
        foreach (ContainerUI<OutfitInfo> containerUI in outfitsOwnedByPlayerItemsUI.stackUIPool) {
            containerUI.GetComponent<Button>().interactable = containerUI.productInfo 
                                                              && !outfitsWornByPlayer.Contains(containerUI.productInfo);
        }
        UpdatePlayerMoney();
        OnSellModeActivate.Invoke();
    }
    
    public void OnOutfitContainerPressedInBuyMode(OutfitContainerUI outfitContainerUI) {
        itemToBuyDisplay.gameObject.SetActive(true);
        itemToBuyDisplay.LoadInfo(outfitContainerUI.productInfo);
        float goldAfterTransaction = shopManager.GetPlayerMoney() - outfitContainerUI.productInfo.cost;
        UpdatePlayerMoney();
        buyTransactionTotalCostText.text = "-" + outfitContainerUI.productInfo.cost;
        // UI update according to money.
        bool enoughMoneyForTransaction = goldAfterTransaction >= 0;
        notEnoughMoneyNotification.gameObject.SetActive(!enoughMoneyForTransaction);
        buyButton.interactable = enoughMoneyForTransaction;
        buyAndEquipButton.interactable = enoughMoneyForTransaction;
    }
    
    public void TryAddItemToSellFromContainer(OutfitContainerUI outfitContainerUI) {
        // Change the container's container and make it non clickable
        outfitContainerUI.gameObject.SetActive(false);
        // Update the UI accordingly
        _outfitsToBeSoldByPlayer.Add(outfitContainerUI.productInfo);
        outfitsToBeSoldByPlayerItemsUI.LoadInfo(_outfitsToBeSoldByPlayer);
        sellTransactionEarningsText.text = _outfitsToBeSoldByPlayer.Count>0 ? 
            "t" + _outfitsToBeSoldByPlayer.Sum(x=>x.cost) : "";
        sellButton.interactable = _outfitsToBeSoldByPlayer.Count > 0;
    }
    
    /// <summary>
    /// Called when an item being sold is pressed
    /// </summary>
    /// <param name="outfitContainerUI"></param>
    public void TryRemoveItemToSellFromContainer(OutfitContainerUI outfitContainerUI) {
        outfitContainerUI.gameObject.SetActive(false);
        // Change the container's container and make it non clickable
        outfitContainerUI.transform.parent = outfitsOwnedByPlayerItemsUI.container.transform;
        // Update the UI accordingly
        _outfitsToBeSoldByPlayer.Remove(outfitContainerUI.productInfo);
        outfitsToBeSoldByPlayerItemsUI.LoadInfo(_outfitsToBeSoldByPlayer);
        // Calculate the outfits owned by the player not currently being tried to sold by the player
        List<OutfitInfo> validOutfitsToSell =
            shopManager.GetOwnedOutfits().FindAll(x => !_outfitsToBeSoldByPlayer.Contains(x));
        outfitsOwnedByPlayerItemsUI.LoadInfo(validOutfitsToSell);
        sellTransactionEarningsText.text = _outfitsToBeSoldByPlayer.Count>0 ? 
            "t" + _outfitsToBeSoldByPlayer.Sum(x=>x.cost) : "";
        sellButton.interactable = _outfitsToBeSoldByPlayer.Count > 0;
    }
    
    /// <summary>
    /// Tries to buy the current item in the item to buy container UI
    /// </summary>
    public void TryBuyCurrentlySelected() {
        // If transaction successful, 'activate' buy mode again, so it updates all the values on the UI.
        if (shopManager.TryBuy(itemToBuyDisplay.productInfo) == InventoryTransaction.TRANSACTION_SUCCESS) {
            ActivateBuyMode();
        }
    }
    
    /// <summary>
    /// Tries to sell all the items in the current outfits to sell items UI
    /// </summary>
    public void TrySellAll() {
        if (shopManager.TrySell(_outfitsToBeSoldByPlayer.ConvertAll(Converter))==InventoryTransaction.TRANSACTION_SUCCESS) {
            ActivateSellMode();
        }
    }

    private ProductInfo Converter(OutfitInfo input) {
        return input;
    }

    public void UpdatePlayerMoney() {
        buyModePlayerMoneyText.text = shopManager.GetPlayerMoney() + "";
        sellModePlayerMoneyText.text = shopManager.GetPlayerMoney() + "";
    }

    public void CancelPurchase() {
        OnTransactionCompleted.Invoke();
    }
    
    public void CompletePurchase() {
        OnTransactionCompleted.Invoke();
    }
}
