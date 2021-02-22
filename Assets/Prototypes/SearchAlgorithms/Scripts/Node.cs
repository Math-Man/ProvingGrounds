using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> Connections;

    private void Awake()
    {
        Connections = new List<Node>();
    }

    public void FindConnections() 
    {
        var colliders = STHelper.GetCollidersInRangeByTag(transform.position, NodeManager.CONN_DIST, "node").ToList();

        colliders.ForEach(c =>
        {
            var node = c.gameObject.GetComponent<Node>();
            if (!node.Equals(this))
            {
                Connections.Add(node);

                //Promt added node to find connections as well
                node.Connections.Add(this);
            }

        });

    }

    private void Start()
    {
       
    }

}
