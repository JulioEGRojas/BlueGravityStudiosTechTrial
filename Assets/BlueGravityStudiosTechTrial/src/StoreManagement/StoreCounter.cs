using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StoreCounter : Interactable {

    [SerializeField] private ShopManager shopManager;

    public override void OnInteractorEnter() {
        onInteractorEnterEvent.Invoke();
    }

    public override void OnInteractorExit() {
        onInteractorExitEvent.Invoke();
    }
    
    public override IEnumerator TryInteract(Interactor i) {
        Debug.Log(i + " tried to interact with " + gameObject.name);
        if (true) {
            yield return null;
            Interact(i);
        }
    }

    public override int Interact(Interactor i) {
        shopManager.OnShopRequest(i);
        onInteractionStartedEvent.Invoke();
        return INTERACTION_SUCCESSFUL;
    }
}