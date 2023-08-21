using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductInfo : ScriptableObject {

    public string label;

    public string description;

    public float cost;
    
    /// <summary>
    /// Sprite that will be shown when displaying it in the UI
    /// </summary>
    public Sprite uiSprite;

    /// <summary>
    /// Category this product belongs to
    /// </summary>
    public CategoryEnum category;
}
