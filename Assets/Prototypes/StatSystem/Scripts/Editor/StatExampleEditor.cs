using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SBExample))]
[CanEditMultipleObjects]
public class StatExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SBExample script = (SBExample)target;
        if (GUILayout.Button("Calculate"))
        {
            script.Calculate();
        }


    }
}
