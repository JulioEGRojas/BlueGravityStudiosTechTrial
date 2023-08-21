using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutfitContainerUI : ContainerUI<OutfitInfo> {

    [SerializeField] private Image outfitImage;
    
    [SerializeField] private TextMeshProUGUI outfitName;
    
    [SerializeField] private TextMeshProUGUI costText;
    
    private void Awake() {
        if (!productInfo || !productInfo.uiSprite) {
            return;
        }
        outfitImage.sprite = productInfo.uiSprite;
        outfitName.text = productInfo.label;
    }

    public override void LoadInfo(OutfitInfo newProductInfo) {
        if (!newProductInfo) {
            return;
        }
        productInfo = newProductInfo;
        outfitImage.sprite = productInfo.uiSprite;
        outfitName.text = productInfo.label;
        costText.text = productInfo.cost + "";
    }

    public override void OnContainerHovered() {
    }

    public override void OnContainerSelected() {
    }
}
