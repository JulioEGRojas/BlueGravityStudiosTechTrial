using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObjectDetector<T> : MonoBehaviour where T : MonoBehaviour {
    
    /// <summary>
    /// Entities close to the entity
    /// </summary>
    public List<T> nearbyEntities;

    private void Awake() {
        nearbyEntities = new List<T>();
    }

    public T GetClosestObject() {
        if (nearbyEntities.Count <= 0) {
            return null;
        }
        return nearbyEntities[0];
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent(out T detectedObject)) {
            if (!nearbyEntities.Contains(detectedObject)) {
                nearbyEntities.Add(detectedObject);
                OnObjectDetected(detectedObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent(out T detectedObject)) {
            if (nearbyEntities.Contains(detectedObject)) {
                nearbyEntities.Remove(detectedObject);
                OnObjectExited(detectedObject);
            }
        }
    }

    public abstract void OnObjectDetected(T detectedObject);

    public abstract void OnObjectExited(T detectedObject);
}
