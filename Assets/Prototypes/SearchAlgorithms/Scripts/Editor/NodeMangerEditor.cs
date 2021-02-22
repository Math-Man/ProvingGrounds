using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(NodeManager))]
public class NodeManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NodeManager script = (NodeManager)target;
        if (GUILayout.Button("Find Path"))
        {
            script.FindPath();
        }
    }
}
