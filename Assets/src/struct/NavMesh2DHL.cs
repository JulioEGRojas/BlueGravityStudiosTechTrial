using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class NavMesh2DHL : MonoBehaviour{
    
    public float entityRadius;

    public float horizontalStep;
    
    public float verticalStep;
    
    public float maxDistanceFromOrigin;

    public LayerMask whatIsImpassable;
    
    public LayerMask whatIsNavMesh;
    
    /// <summary>
    /// Areas in which the nav mesh will detect different path costs
    /// </summary>
    public LayerMask navMeshAreas;

    private Graph<Transform> _navMesh;

    public GameObject nodePrefab;
    
    private GameObject _nodeContainer;

    private Transform _initialNode;

    /// <summary>
    /// Point at which the script starts building the nav mesh, in local coordinates
    /// </summary>
    public Transform origin;

    private readonly Vector3[] raycastChecks = {Vector3.up, Vector3.down, Vector3.right, Vector3.left};

    private float[] magnitudes;

    private int _nodeCount;
    
    public void BuildNavMesh() {
        _nodeContainer = new GameObject("Node Container");
        magnitudes = new[] {verticalStep, verticalStep, horizontalStep, horizontalStep};
        _navMesh = new Graph<Transform>();
        _initialNode = Expand(origin.position);
        _nodeContainer.transform.parent = transform;
    }

    public List<Vector3> GetPathTo(Vector3 initialLocation, Vector3 targetLocation) {
        List<Vector3> result = new List<Vector3>();
        Transform initialNode = ClosestNodeTo(initialLocation), finalNode = ClosestNodeTo(targetLocation);
        
        // List of nodes that compose the path to the target location
        List<Transform> pathNodes = _navMesh.AStar(initialNode, finalNode, 
            t => Vector3.Distance(t.position,finalNode.position));
        // Node list is preprocessed
        
        // All node positions are dadded to the result list
        Vector3 pathPosition;
        foreach (Transform t in pathNodes) {
            pathPosition = t.position;
            result.Add(new Vector3(pathPosition.x,pathPosition.y,0));
        }
        return result;
    }

    public bool DisconnectsIfRemoved(Transform node) {
        return false;
    }

    public GameObject CreateNode(Vector3 newPosition) {
        GameObject newNode = Instantiate(nodePrefab, newPosition, Quaternion.identity);
        newNode.layer = (int) Mathf.Log(whatIsNavMesh.value, 2);;
        newNode.transform.parent = _nodeContainer.transform;
        newNode.name = "NavMeshNode#" + _nodeCount;
        _nodeCount++;
        return newNode;
    }

    public Transform ClosestNodeTo(Vector3 position) {
        Transform closest = null;
        float closestValue = Mathf.Infinity, currentValue;
        foreach (Transform t in _navMesh) {
            currentValue = Vector3.Distance(position, t.position);
            if (currentValue < closestValue) {
                closestValue = currentValue;
                closest = t;
            }
        }

        return closest;
    }

    /// <summary>
    /// Builds the nave mesh using steps of the given magnitudes until the end of level or until a fixed distance is reached
    /// </summary>
    public void BuildNavMesh(Vector3 origin) {
        _navMesh = new Graph<Transform>();
        _initialNode = Expand(origin);
        _nodeContainer.transform.parent = transform;
    }

    public void OnObstacleMoved(Vector3 originalPosition, Vector3 newPosition) {
        // Get and erase the nav mesh node at new position, and delete it from their neighbours list
        Transform nodeAtNewPosition = Physics2D.OverlapCircle(newPosition, 0.3f, whatIsNavMesh).transform;
        // Delete node at obstacle new position
        if (nodeAtNewPosition) {
            // Neighbors have to be disconnected if they are disconnected from the graph, so that the path finding
            // algorithms don't try to find a path to a disconnected location
            List<Transform> neighbors = _navMesh.GetNeighbors(nodeAtNewPosition);
            //
            _navMesh.Remove(nodeAtNewPosition);
            Destroy(nodeAtNewPosition.gameObject);
            neighbors.ForEach(PruneIfDisconnectedFromOrigin);
            // Create new node at original position, and connect it with the rest of the nav mesh
            Expand(originalPosition);
        }
    }

    public void PruneIfDisconnectedFromOrigin(Transform initialElement) {
        HashSet<Transform> subGraph = _navMesh.Explore(initialElement);
        if (!subGraph.Contains(_initialNode)) {
            foreach (Transform navMeshNode in subGraph) {
                _navMesh.Remove(navMeshNode);
                Destroy(navMeshNode.gameObject);
            }
        }
    }

    /// <summary>
    /// Tries to expand the navmesh from the given position
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The initial node from the expansion</returns>
    public Transform Expand(Vector3 position) {
        List<Transform> toExploreQueue = new List<Transform>();
        Transform expansionNode = CreateNode(position).transform;
        Transform currentNode, newNode, collidedNode;
        Vector3 nextLocation = position, currentLocation;
        float connectionCost;
        RaycastHit2D hit;
        toExploreQueue.Add(expansionNode);
        _navMesh.Add(expansionNode);
        // Nave Mesh is built placing multiple raycasts on the x and y axis until an end is reached
        while (toExploreQueue.Count>0) {
            currentNode = toExploreQueue[0];
            currentLocation = currentNode.position;
            toExploreQueue.Remove(currentNode);
            // Collider of currentNode is deactivated so that raycast doesn't detect self
            currentNode.GetComponent<Collider2D>().enabled = false;
            // Raycasts are checked
            for (int i = 0; i < raycastChecks.Length; i++) {
                nextLocation = currentLocation + raycastChecks[i] * magnitudes[i];
                if (Vector3.Distance(origin.position, nextLocation) > maxDistanceFromOrigin) {
                    continue;
                }
                hit = Physics2D.Linecast(currentLocation, nextLocation, whatIsNavMesh);
                if (hit) {
                    collidedNode = hit.collider.transform;
                    if (!_navMesh.AreNeighbors(currentNode, collidedNode)) {
                        _navMesh.Connect(currentNode,collidedNode, new Edge(Vector3.Distance
                            (currentNode.position,collidedNode.position)));
                    }
                }
                else {
                    hit = Physics2D.Linecast(currentLocation, nextLocation, whatIsImpassable);
                    if (!hit) {
                        newNode = CreateNode(nextLocation).transform;
                        _navMesh.Add(newNode.transform);
                        hit = Physics2D.Linecast(currentLocation, nextLocation, navMeshAreas);
                        // Cost calculation
                        float distanceToNewNode = Vector3.Distance(currentNode.position, newNode.position);
                        if (hit) {
                            float distanceToIntersection = Vector3.Distance(hit.point, currentLocation);
                            connectionCost = distanceToIntersection + 
                                             (distanceToNewNode - distanceToIntersection) * 
                                             hit.collider.GetComponent<NavMesh2DArea>().areaCost;
                        } else {
                            connectionCost = distanceToNewNode;
                        }
                        _navMesh.Connect(currentNode,newNode, new Edge(connectionCost));
                        toExploreQueue.Add(newNode); 
                    }
                }
                // Collider is re enabled before updating the current node reference
            }
            currentNode.GetComponent<Collider2D>().enabled = true;
        }

        return expansionNode;
    }

    public void Hide() {
        foreach (Transform node in _navMesh) {
            if (node.TryGetComponent(out SpriteRenderer spriteRenderer)) {
                spriteRenderer.enabled = false;
            }
        }
    }
    
    public void Deactivate() {
        foreach (Transform node in _navMesh) {
            node.gameObject.SetActive(false);
        }
    }

    public void PaintNavMesh(Color color) {
        List<Transform> neighbors;
        foreach (Transform node in _navMesh) {
            neighbors = _navMesh.GetNeighbors(node);
            foreach (Transform neighbor in neighbors) {
                Debug.DrawLine(node.position, neighbor.position,color);
            }
        }
    }
}
