using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is lifted from another one of my projects fyi
/// </summary>
public class PathFinder 
{
    //RecursiveBFS
    public static PathObject FindPathBFS(Node current, Node end, List<Node> visited, bool initial = false, PathObject currentPath = null)
    {
        if (initial)
        {
            currentPath = new PathObject();
            currentPath.AddNode(current);
            visited.Add(current);
        }

        if (current.Equals(end))
        {
            currentPath.PrintPath();
            return currentPath;
        }
        else if (current.Connections.Contains(end))
        {
            currentPath.AddNode(end);
            currentPath.PrintPath();
            return currentPath;
        }

        //Sort connections by distance to the end
        var sortedConnectionsList = current
            .Connections
            .OrderBy(n => Vector3.Distance(n.transform.position, end.transform.position));

        foreach (Node connNode in sortedConnectionsList)
        {
            if (!visited.Contains(connNode))
            {
                visited.Add(connNode);
                currentPath.AddNode(connNode);
                var result = FindPathBFS(connNode, end, visited, false, currentPath);

                if (result != null)
                    return result;

            }
        }
        currentPath.RemoveLast();
        return null;
    }
}


public class PathObject : System.ICloneable
{
    public List<Node> NodesList { get; private set; }

    public PathObject()
    {
        NodesList = new List<Node>();
    }



    public void AddNode(Node node)
    {
        NodesList.Add(node);
    }

    public void RemoveLast()
    {
        NodesList.RemoveAt(NodesList.Count - 1);
    }

    public Node GetLastNode()
    {
        return NodesList[NodesList.Count - 1];
    }


    /// <summary>
    /// Is this path relevant to this node
    /// "Relevant" means if this node is one of the nodes in this path or connected to one of the nodes in this path
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public bool IsRelevant(Node rnode)
    {
        foreach (Node node in NodesList)
        {
            if (node.Equals(rnode))
            {
                return true;
            }
            else
            {
                if (node.Connections.Contains(rnode))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public object Clone()
    {
        PathObject clone = new PathObject();
        clone.NodesList.Clear();
        clone.NodesList.AddRange(this.NodesList);
        return clone;
    }


    public void PrintPath()
    {
        string path = "";
        foreach (Node n in NodesList)
        {
            path += n.name + " -> ";
        }
        Debug.Log(path);
    }
}