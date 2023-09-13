using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionArea : MonoBehaviour {
    
    public Interactable interactable;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out Interactor i)) {
            interactable.Interact(i);
        }
    }
}
