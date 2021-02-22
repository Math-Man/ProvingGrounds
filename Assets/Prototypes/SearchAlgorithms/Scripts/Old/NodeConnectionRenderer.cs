using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(NodeScript))]
public class NodeConnectionRenderer : MonoBehaviour
{
    private NodeScript node;
    private List<ConnectorRenderer> connRenderers;

    private void Awake()
    {
        node = gameObject.GetComponent<NodeScript>();
        connRenderers = new List<ConnectorRenderer>();
    }

    void Start()
    {
       
    }

    void Update()
    {

    }



    public void CreateLineRenderer(NodeEdge edge) 
    {
        GameObject sub = new GameObject();
        sub.transform.SetParent(this.transform, false);

        LineRenderer lr = sub.AddComponent<LineRenderer>();
        ConnectorRenderer cr = new ConnectorRenderer(edge)
        {
            renderer = lr
        };

        connRenderers.Add(cr);
        lr.SetPosition(0, edge.conn1.ScriptRef.transform.position);
        lr.SetPosition(1, edge.conn2.ScriptRef.transform.position);

    }


    public void RefreshLineRenderers() 
    {
        connRenderers.ForEach(cr => 
        {
            cr.renderer.SetPosition(0, cr.edge.conn1.ScriptRef.transform.position);
            cr.renderer.SetPosition(1, cr.edge.conn2.ScriptRef.transform.position);
        });
    }

    public void RemoveLineRendererByConnection(NodeData connection) 
    {

    }

}

public class ConnectorRenderer 
{
    public NodeEdge edge;
    public LineRenderer renderer;

    public ConnectorRenderer(NodeEdge edge) 
    {
        this.edge = edge;
    }
}
