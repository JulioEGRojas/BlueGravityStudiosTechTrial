using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "OutfitInfo", menuName = "ProductInfo/OutfitInfo")]
public class OutfitInfo : ProductInfo {
    
    /// <summary>
    /// Library of sprites used by the outfit
    /// </summary>
    public SpriteLibraryAsset spriteLibraryAsset;

    /// <summary>
    /// Slot where the outfit is put in the player
    /// </summary>
    public OutfitManager.OutfitSlot outfitSlot;
}
