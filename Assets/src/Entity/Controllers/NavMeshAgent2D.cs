using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshAgent2D : GenericCharacterController {

    /// <summary>
    /// Distnace at which an entity will consider to have arrived to a position
    /// </summary>
    public float stoppingDistance;

    protected Transform target;
    
    public LayerMask whatIsImpassable;

    public bool following;
    
    // Path management
    
    protected NavMesh2DHL _navMesh2D;

    protected Coroutine _goToPathCoroutine;

    public void GoTo(Vector3 position) {
        StartCoroutine(MoveTo(position));
    }
    
    /// <summary>
    /// Moves to the nav mesh point that is the closest to position 
    /// </summary>
    /// <param name="finalPosition"></param>
    /// <returns></returns>
    public override IEnumerator MoveTo(Vector3 finalPosition) {
        //List<Vector3> pathToPosition = _levelManager.GetPathFromTo(transform.position, position);
        Vector3 directionToDestiny;
        while (Vector3.Distance(finalPosition,transform.position)>stoppingDistance) {
            directionToDestiny = Vector3.Normalize(finalPosition - transform.position);
            //Debug.Log(directionToDestiny);
            xMove = directionToDestiny.x;
            yMove = directionToDestiny.y;
            yield return new WaitForFixedUpdate();
        }
        _animator.SetFloat(LASTXMOVE_ANIM_ID,Mathf.RoundToInt(xMove));
        _animator.SetFloat(LASTYMOVE_ANIM_ID,Mathf.RoundToInt(yMove));
        xMove = 0;
        yMove = 0;
    }

    public void StartFollowing(Transform target) {
        this.target = target;
        following = true;
        StartCoroutine(Follow(target));
    }

    public void StopFollowing() {
        // Controller goes to last known target position
        GoTo(target.position);
        target = null;
        following = false;
    }
    
    public override void SetDestination(Transform targetDestination) {
        destination = targetDestination.position;
        PathUpdate(targetDestination.right);
    }

    public override void SetDestination(Vector3 destination) {
        this.destination = destination;
        PathUpdate();
    }

    public void PathUpdate() {
        if (_goToPathCoroutine != null) {
            StopCoroutine(_goToPathCoroutine);
        }
        _goToPathCoroutine = StartCoroutine(FollowPath(_navMesh2D.GetPathTo(transform.position, destination)));
    }
    
    public void PathUpdate(Vector3 rightLookAt) {
        if (_goToPathCoroutine != null) {
            StopCoroutine(_goToPathCoroutine);
        }
        
        _goToPathCoroutine = StartCoroutine(FollowPath(_navMesh2D.GetPathTo(transform.position, destination),
            rightLookAt));
    }

    public IEnumerator FollowPath(List<Vector3> path) {
        arrived = false;
        while (path.Count!=0) {
            yield return MoveTo(path[0]);
            path.RemoveAt(0);
        }
        xMove = 0;
        yMove = 0;
        arrived = true;
        _goToPathCoroutine = null;
    }
    
    public IEnumerator FollowPath(List<Vector3> path, Vector3 rightLookAt) {
        yield return FollowPath(path);
        LookAt(rightLookAt);
    }

    public IEnumerator Follow(Transform toFollow) {
        while (following) {
            if (!Physics2D.Linecast(transform.position, toFollow.position, whatIsImpassable)) {
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetMainNavMesh(NavMesh2DHL navMesh2D) {
        _navMesh2D = navMesh2D;
    }
}
