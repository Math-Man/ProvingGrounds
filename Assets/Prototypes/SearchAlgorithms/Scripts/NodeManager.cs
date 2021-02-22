using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public const float CONN_DIST = 6f;

    private List<Node> Nodes;
    private List<NodeConnectionPair> Pairs;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject packetPrefab;

    [SerializeField] private Node SOURCE;
    [SerializeField] private Node GOAL;

    private void Awake()
    {
        Nodes = new List<Node>();
        Pairs = new List<NodeConnectionPair>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            InstantiateNode();
        }    
    }

    public void FindPath()
    {
        if (SOURCE == null || GOAL == null)
            return;

        var result = PathFinder.FindPathBFS(SOURCE, GOAL, new List<Node>(), true);

        if (result != null)
        {
            Debug.Log("Path found firing packet");
            FirePacket(result);
        }
        else 
        {
            Debug.Log("No path found");
        }
    }

    public void FirePacket(PathObject path) 
    {
        var p = Instantiate(packetPrefab);
        p.transform.position = path.NodesList[0].transform.position;
        var np = p.GetComponent<NodePacket>();
        np.path = path;
    }

    public void InstantiateNode() 
    {
        var obj = Instantiate(prefab);
        obj.name = "NODE: " + obj.GetHashCode();
        obj.transform.position = STHelper.GetMouseWorldPosition(Camera.main);
        var node = obj.GetComponent<Node>();
        Nodes.Add(node);
        node.FindConnections();
        AddPairsFromNodeConnections(node);

        if (SOURCE == null)
            SOURCE = node;
        GOAL = node;

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(STHelper.GetMouseWorldPosition(Camera.main), CONN_DIST);

        Pairs?.ForEach(p => 
        {
            //Handles.Label((p.n1.transform.position + p.n2.transform.position)/2, p.n1.gameObject.GetHashCode() + "<->" + p.n2.gameObject.GetHashCode());
            Gizmos.DrawLine(p.n1.transform.position, p.n2.transform.position);
        });
    }

    private void AddPairsFromNodeConnections(Node node) 
    {
        node.Connections.ForEach(n => 
        {
            TryAddPairToList(new NodeConnectionPair(node, n));
        });
    }

    private bool TryAddPairToList(NodeConnectionPair pair) 
    {
        if (PairsContain(pair))
            return false;

        Pairs.Add(pair);
       // Debug.Log("Added: " + pair.n1.GetHashCode() + "|" + pair.n2.GetHashCode());
        return true;
    }

    private bool PairsContain(NodeConnectionPair pairToCheck) 
    {
        var result = Pairs.Any(p => p.isEqual(pairToCheck));
        return result;
    }

    private struct NodeConnectionPair 
    {
        public Node n1;
        public Node n2;

        public NodeConnectionPair(Node n1, Node n2) 
        {
            this.n1 = n1;
            this.n2 = n2;
        }

        public bool isEqual(Node n1, Node n2) 
        {
            if ((n1.Equals(n2) && n2.Equals(n1)) || (n1.Equals(n1) && n2.Equals(n2))) 
            {
                return true;
            }
            return false;
        }

        public bool isEqual(NodeConnectionPair pair)
        {
            if ((pair.n1.Equals(n2) && pair.n2.Equals(n1)) || (pair.n1.Equals(n1) && pair.n2.Equals(n2)))
            {
                return true;
            }
            return false;
        }

    }

}
