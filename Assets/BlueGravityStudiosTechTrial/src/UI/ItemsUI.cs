using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsUI<T> : MonoBehaviour where T: ProductInfo {

    public GameObject container;
    
    public ContainerUI<T> itemStackUIPrefab;
    
    public List<ContainerUI<T>> stackUIPool = new List<ContainerUI<T>>();

    public int initialUIStacks = 8;

    public bool initialized;

    /// <summary>
    /// Items UI MUST be initialized AFTER ONE FRAME. Otherwise, they will loop due to Unity UI
    /// systems not being initialized yet
    /// </summary>
    /// <returns></returns>
    public void Initialize() {
        CreateAdditionalStacks(initialUIStacks);
        AccomodateStacksUI();
        initialized = true;
    }

    public void ClearInfo() {
        LoadInfo(new List<T>());
    }

    public virtual void LoadInfo(List<T> infoToShow) {
        // Create missing containers
        if (stackUIPool.Count < infoToShow.Count) {
            CreateAdditionalStacks(infoToShow.Count - stackUIPool.Count);
            AccomodateStacksUI();
        }
        // Populate the inventory
        int i;
        for (i = 0; i < infoToShow.Count; i++) {
            stackUIPool[i].gameObject.SetActive(true);
            stackUIPool[i].GetComponent<RectTransform>().SetParent(container.transform);
            stackUIPool[i].LoadInfo(infoToShow[i]);
        }
        
        // Deactivate remaining containers
        for (; i < stackUIPool.Count; i++) {
            stackUIPool[i].gameObject.SetActive(false);
        }
    }
    
    public void Clear() {
        for (int i = 0; i < stackUIPool.Count; i++) {
            stackUIPool[i].gameObject.SetActive(false);
        }
    }

    public void AccomodateStacksUI() {
        for (var i = 0; i < stackUIPool.Count; i++) {
            stackUIPool[i].transform.SetParent(container.transform);
        }
    }

    public void CreateAdditionalStacks(int amount) {
        for (int i = 0; i < amount; i++) {
            stackUIPool.Add(Instantiate(itemStackUIPrefab, transform.position, container.transform.rotation,
                container.transform));
        }
    }
}
