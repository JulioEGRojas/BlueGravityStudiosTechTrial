using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerExposer : MonoBehaviour {
 
    [SerializeField]
    private int SortingOrder = 0;
 
    public void OnValidate() {
        Apply();
    }
 
    public void OnEnable() {
        Apply();
        Destroy(this);
    }
 
    private void Apply() {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = SortingOrder;
    }
}
