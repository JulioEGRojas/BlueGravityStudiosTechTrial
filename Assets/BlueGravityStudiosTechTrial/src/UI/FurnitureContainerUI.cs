using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureContainerUI : ContainerUI<FurnitureInfo> {
    
    [SerializeField]
    private Image furnitureImage;
    
    [SerializeField]
    private TextMeshProUGUI furnitureName;
    
    private void Awake() {
        if (productInfo.uiSprite) {
            furnitureImage.sprite = productInfo.uiSprite;
            furnitureName.text = productInfo.label;
        }
    }

    public override void LoadInfo(FurnitureInfo newProductInfo) {
        if (!newProductInfo) {
            return;
        }
        productInfo = newProductInfo;
        furnitureImage.sprite = productInfo.uiSprite;
        furnitureName.text = productInfo.label;
    }

    public override void OnContainerHovered() {
    }

    public override void OnContainerSelected() {
    }
}
