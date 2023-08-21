using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : Construction {
    
    public const int INTERACTION_SUCCESSFUL = 1, INTERACTION_FAILED = -1;

    public InteractionArea interactionArea;

    [SerializeField] protected UnityEvent onInteractorEnterEvent;
    
    [SerializeField] protected UnityEvent onInteractorExitEvent;
    
    [SerializeField] protected UnityEvent onInteractionStartedEvent;
    
    [SerializeField] protected UnityEvent onInteractionFinished;
    
    [SerializeField] protected UnityEvent onInteractionFailedEvent;
    
    public abstract IEnumerator TryInteract(Interactor i);

    public abstract int Interact(Interactor i);
    
    public virtual void OnInteractorEnter() {
    }
    
    public virtual void OnInteractorExit() {
    }
}
