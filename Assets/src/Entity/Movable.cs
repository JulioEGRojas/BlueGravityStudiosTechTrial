using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
public abstract class Movable : MonoBehaviour {

    public NavMeshAgent2D controller;

    protected Animator animator;

    /// <summary>
    /// Sets the deltaMove variable for the animator
    /// </summary>
    /// <param name="move"></param>
    public void SetMove(bool move) {
        animator.SetBool("Moving", move);
    }
    
    public bool IsMoving() {
        return controller.IsMoving();
    }
}
