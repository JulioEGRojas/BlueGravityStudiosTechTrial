using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class for directed graphs
/// </summary>
/// <typeparam name="T"></typeparam>
public class Graph<T> : IEnumerable<T> {
    
    public int Count;

    // Hashtable<Element,Node>
    private Dictionary<T,Node> nodes;

    // Hashtable 
    private HashSet<Edge> edges;

    public Graph()
    {
        this.nodes = new Dictionary<T, Node>();
        this.edges = new HashSet<Edge>();
    }

    public void Add(T element)
    {
        if (Contains(element))
        {
            throw new System.Exception();
        }
        Node new_Node = new Node(element);
        Count++;
        nodes.Add(element, new_Node);
    }

    public void Remove(T element)
    {
        if (!Contains(element))
        {
            throw new System.Exception();
        }
        // Neighbors erase their reference to removed node
        Node n = GetNode(element);
        foreach (Node neighbor in n.neighbors.Keys)
        {
            neighbor.neighbors.Remove(n);
        }
        nodes.Remove(element);
        Count--;
    }

    public bool Contains(T element)
    {
        return nodes.ContainsKey(element);
    }

    public List<T> GetNeighbors(T element)
    {
        List<T> result = new List<T>();
        Node n = GetNode(element);
        foreach (Node neighbor in n.neighbors.Keys)
        {
            result.Add(neighbor.element);
        }
        return result;
    }

    /// <summary>
    /// Determines if element is disconnected in the graph (Has no neighbors)
    /// </summary>
    /// <param name="element">Element to check if connected</param>
    /// <returns>True if disconnected, false otherwise</returns>
    public bool IsDisconnected(T element)
    {
        return GetNode(element).neighbors.Count == 0;
    }

    /// <summary>
    /// Connects node with element1 to node with element2 with the specified object
    /// </summary>
    /// <param name="element1"></param>
    /// <param name="element2"></param>
    /// <param name="edge"></param>
    public void Connect(T element1, T element2, Edge edge)
    {
        Node n1 = GetNode(element1), n2 = GetNode(element2);
        if (AreNeighbors(element1, element2))
        {
            Debug.Log("Elementos ya conectados");
            return;
        }
        n1.neighbors.Add(n2, edge);
        n2.neighbors.Add(n1, edge);
        edges.Add(edge);
    }

    public void Disconnect(T element1, T element2)
    {
        Node n1 = GetNode(element1), n2 = GetNode(element2);
        if (!AreNeighbors(element1, element2))
        {
            Debug.Log("Elementos no conectados");
            return;
        }
        Edge edge = n1.neighbors[n2];
        edges.Remove(edge);
        n1.neighbors.Remove(n2);
        n2.neighbors.Remove(n1);
    }

    public bool AreNeighbors(T element1, T element2)
    {
        Node n1 = GetNode(element1), n2 = GetNode(element2);
        return (n1.neighbors.ContainsKey(n2) && n2.neighbors.ContainsKey(n1));
    }

    /// <summary>
    /// Updates the node containing element1 so that now contains element2
    /// </summary>
    /// <param name="element1"></param>
    /// <param name="element2"></param>
    public void Update(T element1, T element2) {
        Node n1 = GetNode(element1);
        n1.element = element2;
        nodes.Remove(element1);
        nodes.Add(element2,n1);
    }

    /// <summary>
    /// Connects the two graph values with the connection_Value value
    /// </summary>
    /// <param name="element1"></param>
    /// <param name="element2"></param>
    /// <param name="connection_Valute"></param>
    public void ConnectWithValue(T element1, T element2, System.Object new_Connection_Value)
    {
        if (AreNeighbors(element1, element2))
        {
            ((Edge)GetNode(element1).neighbors[GetNode(element2)]).element = new_Connection_Value;
            ((Edge)GetNode(element2).neighbors[GetNode(element1)]).element = new_Connection_Value;
            return;
        } Debug.Log("Error, values aren't connected");
    }

    public System.Object GetConnectionValue(T element1, T element2)
    {
        if (AreNeighbors(element1, element2))
        {
            return ((Edge)GetNode(element1).neighbors[GetNode(element2)]).element;
        } Debug.Log("Error, values aren't connected");
        return null;
    }

    public bool ConnectionValued(T element1, T element2)
    {
        if (AreNeighbors(element1, element2))
        {
            return ((Edge)GetNode(element1).neighbors[GetNode(element2)]).element != null;
        } return false;
    }    

    /// <summary>
    /// Gets the edge that connects element1 and element2
    /// </summary>
    /// <param name="element1"></param>
    /// <param name="element2"></param>
    /// <returns></returns>
    public Edge GetEdge(T element1, T element2)
    {
        if (AreNeighbors(element1, element2))
        {
            return (Edge)GetNode(element1).neighbors[GetNode(element2)];
        } return null;
    }

    public HashSet<Edge> GetEdges()
    {
        return edges;
    }

    private Node GetNode(T element) {
        return nodes[element];
    }
    
    public List<T> AStar(T origin, T destiny, Func<T,float> heuristicFunction) {
        // Set all f,g and h values
        foreach (KeyValuePair<T,Node> valuePair in nodes) {
            Node n = valuePair.Value;
            n.parent = null;
            n.gValue = Mathf.Infinity;
            n.hValue = heuristicFunction(n.element);
            n.fValue = n.gValue;
        }
        Node startNode = GetNode(origin), targetNode = GetNode(destiny), current;
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        startNode.gValue = 0;
        startNode.fValue = startNode.hValue;
        //calculates path for pathfinding
        while (openSet.Count > 0) {
            // Find lowest FCost
            current = openSet.Min();
            openSet.Remove(current);
            closedSet.Add(current);
            //If target found, retrace path
            if (current == targetNode) {
                return RetracePath(startNode, targetNode);
            }
            //adds neighbor nodes to openSet
            Node[] currentNeighbors = current.neighbors.Keys.ToArray();
            foreach (Node neighbor in currentNeighbors) {
                // If already explored, ignore it
                if (closedSet.Contains(neighbor)) {
                    continue;
                }
                // If estimated cost to neighbor is now less, 
                float costToNeighbor = current.gValue + current.neighbors[neighbor].connectionValue;
                if (costToNeighbor < neighbor.gValue || !openSet.Contains(neighbor)) {
                    neighbor.gValue = costToNeighbor;
                    neighbor.fValue = neighbor.gValue + neighbor.hValue;
                    neighbor.parent = current;
                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        // Target node is converted to the parented node that is closest to the destiny.
        targetNode = GetNode(GenUtilities.GetMin<T>(nodes.Values.Where(x => x.parent != null).Select(x => x.element),
            heuristicFunction));
        return RetracePath(startNode, targetNode);
    }
    
    private List<T> RetracePath(Node startNode, Node endNode) {
        List<T> path = new List<T>();
        Node currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode.element);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    public IEnumerator<T> GetEnumerator()
    {
        List<T> to_Iterate = new List<T>();
        foreach(Node n in nodes.Values)
        {
            to_Iterate.Add(n.element);
        }
        return to_Iterate.GetEnumerator();
    }

    // WARNING : UNCHECKED
    public List<T> BFS(T initial_Element, int levelsToExplore)
    {
        List<T> result = new List<T>();
        Node current = GetNode(initial_Element);
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        List<Node> nextBFSElements = new List<Node>();
        queue.Enqueue(current);
        closedSet.Add(current);
        int currentLevel = 0;
        // BFS
        while (queue.Count != 0 && currentLevel<levelsToExplore) {
            while (queue.Count > 0) {
                current = queue.Dequeue();
                result.Add(current.element);
                foreach (Node neighbor in current.neighbors.Keys) {
                    if (!closedSet.Contains(neighbor)) {
                        nextBFSElements.Add(neighbor);
                        closedSet.Add(neighbor);
                    }
                }
            }
            nextBFSElements.ForEach(x=>queue.Enqueue(x));
            currentLevel++;
        }
        return result;
    }
    
    public List<T> BFS(T initial_Element) {
        List<T> result = new List<T>();
        Node current = GetNode(initial_Element);
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        queue.Enqueue(current);
        closedSet.Add(current);
        // BFS
        while (queue.Count != 0) {
            current = queue.Dequeue();
            result.Add(current.element);
            foreach (Node neighbor in current.neighbors.Keys) {
                if (!closedSet.Contains(neighbor)) {
                    queue.Enqueue(neighbor);
                    closedSet.Add(neighbor);
                }
            }
        }
        return result;
    }
    
    /// <summary>
    /// Obtains all the nodes that form the subgraph that 'initialElement' is part of. 
    /// </summary>
    /// <param name="initialElement"></param>
    /// <returns></returns>
    public HashSet<T> Explore(T initialElement) {
        return new HashSet<T>(BFS(initialElement));
    }

    public int GetConexComponents() {
        List<Node> openSet = new List<Node>();
        HashSet<Node> currentNodeClosedSet = new HashSet<Node>();
        Stack<Node> currentNodeOpenSet = new Stack<Node>();
        Node currentNode = null;
        // Set all f,g and h values
        foreach (KeyValuePair<T,Node> valuePair in nodes) {
            Node n = valuePair.Value;
            openSet.Add(n);
            // Any node will do
            currentNode = n;
        }
        int conexComponents = 0;
        currentNodeOpenSet.Push(currentNode);

        while (openSet.Count>0) {
            while (currentNodeOpenSet.Count>0) {
                currentNode = currentNodeOpenSet.Pop();
                openSet.Remove(currentNode);
                currentNodeClosedSet.Add(currentNode);
                foreach (Node neighbor in currentNode.neighbors.Keys) {
                    if (!currentNodeClosedSet.Contains(neighbor)) {
                        currentNodeOpenSet.Push(neighbor);
                    }
                }
            }
            conexComponents++;
        }
        return conexComponents;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private class Node : IComparable<Node> {
        public T element;
        
        // neighbors<Node,Edge> contains a set with the nodes and the associated edge to the neighbor connection
        public Dictionary<Node,Edge> neighbors;

        public float fValue;
        
        public float gValue;
        
        public float hValue;

        public Node parent;

        public Node(T element)
        {
            this.element = element;
            neighbors = new Dictionary<Node, Edge>();
        }

        public int CompareTo(Node other) {
            if (fValue > other.fValue) return 1;
            if (fValue < other.fValue) return -1;
            return 0;
        }
    }
}