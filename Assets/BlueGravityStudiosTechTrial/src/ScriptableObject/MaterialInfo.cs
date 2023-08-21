using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material Info", menuName = "ProductInfo/MaterialInfo")]
public class MaterialInfo : ProductInfo {

    /// <summary>
    /// Material associated with this texture
    /// </summary>
    public Material material;
}
