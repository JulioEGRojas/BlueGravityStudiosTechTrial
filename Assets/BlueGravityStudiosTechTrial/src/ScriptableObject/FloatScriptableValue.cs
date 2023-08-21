using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Scriptable Float", menuName = "Values/Float")]
public class FloatScriptableValue : ScriptableObject {

    [SerializeField]
    private float value;

    public float Value {
        get => value;
    }

    public EventHandler OnValueChangedEvent;

    public void SetValue(float newValue) {
        value = newValue;
        OnValueChangedEvent?.Invoke(this,null);
    }

    public void SetValue(Slider slider) {
        value = slider.value;
        OnValueChangedEvent?.Invoke(this,null);
    }
}
