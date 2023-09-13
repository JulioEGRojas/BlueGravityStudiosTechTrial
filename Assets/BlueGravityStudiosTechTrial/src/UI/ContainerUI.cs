using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContainerUI<T> : MonoBehaviour where T: ProductInfo {

    public T productInfo;

    public abstract void LoadInfo(T productInfo);

    public abstract void OnContainerHovered();

    public abstract void OnContainerSelected();
}
