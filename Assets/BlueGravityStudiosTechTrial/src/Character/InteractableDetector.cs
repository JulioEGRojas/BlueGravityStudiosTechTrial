using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : ObjectDetector<Interactable> {
    
    public override void OnObjectDetected(Interactable detectedObject) {
        detectedObject.OnInteractorEnter();
    }

    public override void OnObjectExited(Interactable detectedObject) {
        detectedObject.OnInteractorExit();
    }
}
