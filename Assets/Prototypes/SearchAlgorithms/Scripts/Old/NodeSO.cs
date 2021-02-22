using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node", menuName ="SO/PF/Node", order =1)]
public class NodeSO : ScriptableObject
{
    public NodeType Type;
    public float ConnectionMaxDis = 500;
    public int ConnectionMaxCount = 6;
    public Color ConnectionColour = Color.white;
}


public enum NodeType 
{
    TYPE1,
    TYPE2,
    TYPE3
}