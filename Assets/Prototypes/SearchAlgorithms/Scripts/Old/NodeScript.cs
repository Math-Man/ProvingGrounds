using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NodeScript : MonoBehaviour
{
    private const string NODE_TAG = "node";
    public NodeData Data { get; private set; }
    public NodeConnectionRenderer ConnectionRenderer { get; private set; }

    public Collider SelfCollider { get; private set; }

    [SerializeField] private NodeSO TypeSO;

    private void Awake()
    {
        this.Data = new NodeData(this);
        Data.NodeSO = TypeSO;
        SelfCollider = gameObject.GetComponent<Collider>();
        ConnectionRenderer = gameObject.GetComponent<NodeConnectionRenderer>();
    }

    private void Start()
    {
        RefreshConnections();
    }

    private void RefreshConnections() 
    {
        Collider[] orderedNodes = STHelper.GetCollidersInRangeByTag(transform.position, Data.NodeSO.ConnectionMaxDis, NODE_TAG).ToArray();

        //Flush old set in case max count changed for any reason
        Data.Edges = new List<NodeEdge>();

        for (int i = 0; i < Data.NodeSO.ConnectionMaxCount; i++) 
        {
            var otherNode = orderedNodes[i].gameObject.GetComponent<NodeScript>();

            ConnectionValidationResult validateResult = ValidateConnection(otherNode);
            if (validateResult.success)
            {
                var edge = new NodeEdge(this.Data, otherNode.Data);
                Data.Edges.Add(edge);
                ConnectionRenderer.CreateLineRenderer(edge);
            }
            else
            {
                Debug.Log("Can't make connection: " + validateResult.failState);
            }
        }
    }


    //I like to use a simple class or a struct to keep track of validation actions
    private struct ConnectionValidationResult
    {
        public bool success;
        public int failState;

        public ConnectionValidationResult(bool success, int fail) 
        {
            this.success = success;
            this.failState = fail;
        }
    }

    private ConnectionValidationResult ValidateConnection(NodeScript connection) 
    {
        //Avoid self
        if (SelfCollider.Equals(connection))
            return new ConnectionValidationResult(false, 1);

        //Check reverse Connections
        if (connection.Data.Edges.Any(edge => edge.conn2.Equals(this.Data) && edge.conn1.Equals(connection.Data)))
            return new ConnectionValidationResult(false, 2);

        //TODO: Add other validation constraints; check for types etc.

        return new ConnectionValidationResult(true, 0);

    }

}

//Data classes
public class NodeData
{
    public NodeScript ScriptRef { get; private set; }
    public NodeSO NodeSO { get; set; }

    public List<NodeEdge> Edges;

    public NodeData(NodeScript ns) 
    {
        Edges = new List<NodeEdge>();
        ScriptRef = ns;
    }
}

public class NodeEdge
{
    public NodeData conn1  { get; private set; }  //Node that connects
    public NodeData conn2 { get; private set; }  //Node that it connects to


    //I like to keep  the distance here to avoid recalculating everytime
    public float distance { get; private set; }

    public NodeEdge(NodeData connection, NodeData connector) 
    {
        this.conn2 = connection;
        this.conn1 = connector;

        RecalculateDistance();
    }

    public float RecalculateDistance() 
    {
        distance = (conn2.ScriptRef.transform.position - conn1.ScriptRef.transform.position).magnitude;
        return distance;
    }
} 