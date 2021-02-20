using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Saveable), true)]
public class SaveableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Saveable script = (Saveable)target;
        if (GUILayout.Button("Save"))
        {
            script.saveMe();
        }

    }
}
