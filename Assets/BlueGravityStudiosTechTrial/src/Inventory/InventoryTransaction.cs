using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryTransaction {

    public List<ProductInfo> productsToAdd;
    
    public List<ProductInfo> productsToRemove;

    public float goldToAdd;
    
    public const int TRANSACTION_SUCCESS = 1, TRANSACTION_FAILURE = 0;

    public InventoryTransaction(List<ProductInfo> productsToAdd, List<ProductInfo> productsToRemove, float goldToAdd) {
        this.productsToAdd = productsToAdd;
        this.productsToRemove = productsToRemove;
        this.goldToAdd = goldToAdd;
    }
}
