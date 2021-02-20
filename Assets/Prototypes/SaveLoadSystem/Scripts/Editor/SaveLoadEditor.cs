using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SceneSaveLoad))]
public class SaveLoadEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneSaveLoad script = (SceneSaveLoad)target;
        if (GUILayout.Button("Create")) 
        {
            script.CreateRandom();
        }

        if (GUILayout.Button("Load"))
        {
            script.LoadSaveable();
        }

        if (GUILayout.Button("Wipe"))
        {
            script.WipeAll();
        }

        if (GUILayout.Button("Delete Save File"))
        {
            script.ClearSaveFile();
        }

    }
}
