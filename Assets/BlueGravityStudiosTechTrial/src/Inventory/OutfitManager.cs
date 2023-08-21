using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class OutfitManager : MonoBehaviour {

    public enum OutfitSlot {
        HAT,
        HAIR,
        TOP,
        BOTTOM,
        SHOES
    }

    public GameObject hatGameObject;

    public GameObject hairGameObject;

    public GameObject topGameObject;
    
    public GameObject bottomGameObject;
    
    public GameObject shoesGameObject;

    /// <summary>
    /// Outfits currently worn
    /// </summary>
    public List<OutfitInfo> wornOutfits;

    public void EquipOutfit(OutfitInfo outfitToEquip) {
        GameObject toReplace = null;
        // Remove the outfit in the slot the player is going to use.
        wornOutfits.RemoveAll(x => x.outfitSlot == outfitToEquip.outfitSlot);
        wornOutfits.Add(outfitToEquip);
        switch (outfitToEquip.outfitSlot) {
            default:
            case OutfitSlot.HAT:
                toReplace = hatGameObject;
                break;
            case OutfitSlot.HAIR:
                toReplace = hairGameObject;
                break;
            case OutfitSlot.TOP:
                toReplace = topGameObject;
                break;
            case OutfitSlot.BOTTOM:
                toReplace = bottomGameObject;
                break;
            case OutfitSlot.SHOES:
                toReplace = shoesGameObject;
                break;
        }
        // Replace the sprite library values on the gameObject to replace
        SpriteLibraryAsset replacingLibrary = outfitToEquip.spriteLibraryAsset;
        toReplace.GetComponent<SpriteLibrary>().spriteLibraryAsset = replacingLibrary;
        toReplace.GetComponent<SpriteLibrary>().RefreshSpriteResolvers();
    }
}