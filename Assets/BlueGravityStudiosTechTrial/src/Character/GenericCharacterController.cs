using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCharacterController : MonoBehaviour {
    
    /// <summary>
    /// Units per second the entity will move each fixed update
    /// </summary>
    public float verticalSpeed, horizontalSpeed;
    
    private Rigidbody2D rb;

    [SerializeField] private InteractableDetector interactableDetector;

    /// <summary>
    /// Flag that determines if the entity is looking to the right or not
    /// </summary>
    protected bool _lookingRight = true;

    protected float xMove, yMove;

    /// <summary>
    /// Movement that this movable will use to move upwards/downwards
    /// </summary>
    protected Vector2 verticalMovementVector;
    
    public Vector3 destination;

    /// <summary>
    /// Initial direction this character will be looking at
    /// </summary>
    public Vector3 initialLookAtDirection;

    /// <summary>
    /// Flag that determines if this entity has arrived to its destination
    /// </summary>
    public bool arrived;

    protected Animator _animator;

    public static readonly int XMOVE_ANIM_ID = Animator.StringToHash("xMove"), YMOVE_ANIM_ID = Animator.StringToHash("yMove"),
        LASTXMOVE_ANIM_ID = Animator.StringToHash("lastXMove"), LASTYMOVE_ANIM_ID = Animator.StringToHash("lastYMove");

    protected virtual void Awake() {
        // Since the entity manages the rigidbody movement, it must be in the same gameObject to move the child components
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        LookAt(initialLookAtDirection);
    }

    public void OnInteractionTry() {
        Interactable closestInteractable = interactableDetector.GetClosestObject();
        if (closestInteractable!=null) {
            Debug.Log("interaction");
            closestInteractable.StartCoroutine(closestInteractable.TryInteract(null));
        }
    }

    public virtual void Stop() {
        StopAllCoroutines();
        xMove = 0;
        yMove = 0;
        rb.velocity = GetVelocityVector();
    }
    
    public virtual void FixedUpdate() {
        if (_lookingRight && xMove < 0) {
            Flip();
        } else if (!_lookingRight && xMove > 0) {
            Flip();
        }
        _animator.SetFloat(XMOVE_ANIM_ID,Mathf.RoundToInt(xMove));
        _animator.SetFloat(YMOVE_ANIM_ID,Mathf.RoundToInt(yMove));
        rb.velocity = GetVelocityVector();
    }
    
    public virtual void SetDestination(Transform destination) {
        this.destination = destination.position;
    }

    public virtual void SetDestination(Vector3 position) {
        destination = position;
    }
    
    public virtual IEnumerator MoveTo(Vector3 finalPosition) {
        destination = finalPosition;
        yield return null;
    }

    public bool IsMoving() {
        return xMove != 0 || verticalMovementVector.y!=0;
    }
    
    /// <summary>
    /// Tries to look at the given right direction
    /// </summary>
    /// <param name="right"></param>
    public void LookAt(Vector3 right) {
        _animator.SetFloat(LASTXMOVE_ANIM_ID,Mathf.RoundToInt(right.x));
        _animator.SetFloat(LASTYMOVE_ANIM_ID,Mathf.RoundToInt(right.y));
        if (_lookingRight && right.x < 0) {
            Flip();
        } else if (!_lookingRight && right.x > 0) {
            Flip();
        }
    }

    public void SetX(float newValue) {
        xMove = newValue;
    }
    
    public void SetY(float newValue) {
        yMove = newValue;
    }

    public void SetXYSpeed(float newSpeed) {
        horizontalSpeed = newSpeed;
        verticalSpeed = newSpeed;
    }
    
    protected void Flip() {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        _lookingRight = !_lookingRight;
    }

    public Vector2 GetDirectionVector() {
        return new Vector2(xMove,yMove);
    }
    
    public Vector2 GetVelocityVector() {
        return new Vector2(xMove * horizontalSpeed,yMove * verticalSpeed);
    }
}
