using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Bool", menuName = "Values/String")]
public class StringScriptableValue : ScriptableObject {

    public string value;

    public void SetValue(string newValue) {
        value = newValue;
    }
}
