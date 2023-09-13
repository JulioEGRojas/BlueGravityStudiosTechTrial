using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDataSet<T> : ScriptableObject where T : ProductInfo {

    public List<T> items;

    /// <summary>
    /// Obtains the item with the same label as s
    /// </summary>
    /// <param name="s"></param>
    public T GetFromLabel(string s) {
        return items.Find(x => x.label.Equals(s));
    }
}
