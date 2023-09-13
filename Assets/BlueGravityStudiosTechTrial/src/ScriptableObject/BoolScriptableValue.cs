using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Bool", menuName = "Values/Bool")]
public class BoolScriptableValue : ScriptableObject {

    public bool value;

    public void SetValue(bool newValue) {
        value = newValue;
    }

    public void Toggle() {
        value = !value;
    }
}
