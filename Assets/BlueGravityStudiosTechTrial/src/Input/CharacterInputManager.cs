using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInputManager : MonoBehaviour {

    public GenericCharacterController characterToControl;

    public CharacterInventory characterInventory;

    private Vector3 _movementVector;
    
    protected void Update() {
        _movementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) {
            OnAPressed();
        }
        if (Input.GetKey(KeyCode.D)) {
            OnDPressed();
        }
        if (Input.GetKey(KeyCode.W)) {
            OnWPressed();
        }
        if (Input.GetKey(KeyCode.S)) {
            OnSPressed();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            OnEPressed();
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            OnIPressed();
        }
    }

    /// <summary>
    /// On Fixed Update, update the target diretion of the character to control
    /// </summary>
    private void FixedUpdate() {
        characterToControl.SetX(_movementVector.x);
        characterToControl.SetY(_movementVector.y);
    }

    /// <summary>
    /// On E pressed, try to interact with something
    /// </summary>
    public void OnEPressed() {
        characterToControl.OnInteractionTry();
    }

    public void OnIPressed() {
        characterInventory.DisplayInventoryUI();
    }
    
    public void OnAPressed() {
        _movementVector+=Vector3.left;
    }
    
    public void OnDPressed() {
        _movementVector+=Vector3.right;
    }
    
    public void OnWPressed() {
        _movementVector+=Vector3.up;
    }
    
    public void OnSPressed() {
        _movementVector+=Vector3.down;
    }
    
}
