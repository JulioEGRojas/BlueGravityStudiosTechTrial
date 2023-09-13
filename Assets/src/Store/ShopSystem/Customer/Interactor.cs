using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : Movable {
    
    protected int lastInteractionResult;

    protected Coroutine currentInteractionCoroutine;

    public void SetLastInteractionResult(int lastInteractionResult) {
        this.lastInteractionResult = lastInteractionResult;
    }

    /// <summary>
    /// Tries to interact with the interactable. Last interaction result is updated with the result of the operation.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public virtual IEnumerator Interact(Interactable i) {
        yield return i.TryInteract(this);
    }

    public void GoInteractWith(Interactable i) {
        // Stop previous interaction request
        if (currentInteractionCoroutine!=null) {
            StopCoroutine(currentInteractionCoroutine);
        }
        currentInteractionCoroutine = StartCoroutine(GoInteractWithCor(i, i.transform.position));
    }
    
    /// <summary>
    /// Interacts with the given interactable, on the given position
    /// </summary>
    /// <param name="i"></param>
    /// <param name="interactionLocation"></param>
    public void GoInteractWith(Interactable i, Vector3 interactionLocation) {
        // Stop previous interaction request
        if (currentInteractionCoroutine!=null) {
            StopCoroutine(currentInteractionCoroutine);
        }
        currentInteractionCoroutine = StartCoroutine(GoInteractWithCor(i, interactionLocation));
    }

    public IEnumerator GoInteractWithCor(Interactable i, Vector3 interactionLocation) {
        controller.SetDestination(interactionLocation);
        yield return new WaitUntil(()=>controller.arrived);
        yield return Interact(i);
    }
    
    public IEnumerator GoTo(Transform t) {
        controller.SetDestination(t.position);
        yield return new WaitUntil(()=>controller.arrived);
    }
}
