using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using System;
using System.Runtime.InteropServices;
using UnityEditor.SceneManagement;
using static StatManager;

[CustomEditor(typeof(StatManager))]
[CanEditMultipleObjects]
public class StatManagerEditor : Editor
{
    StatManager StatManagerRef;

    void OnEnable()
    {
        StatManagerRef = (StatManager)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        BuildInspectorUI();
        serializedObject.ApplyModifiedProperties();
    }


    private void BuildInspectorUI()
    {
        EditorGUILayout.BeginHorizontal();
        StatManagerRef.PropCount = EditorGUILayout.IntField("Stat count:", StatManagerRef.PropCount);
        EditorGUILayout.EndHorizontal();

        //Remove surplus?
        while (StatManagerRef.PropertiesToGenerate.Count > StatManagerRef.PropCount)
        {
            StatManagerRef.PropertiesToGenerate.RemoveAt(StatManagerRef.PropertiesToGenerate.Count - 1);
        }

        if (StatManagerRef.PropCount > 0) 
        {
            List<string> names = new List<string>() {"Prop Name", "Default val", "Min val", "Max val"};
            var objectsArray = BuildObjectsArray();

            objectsArray = SGEditorHelpers.BuildPropGrid(StatManagerRef.PropertiesToGenerate.Count, 
                 new List<System.Type>() { typeof(string), typeof(float), typeof(float), typeof(float) },
                 names, 
                 objectsArray);

            RecollectValues(objectsArray);
        }
    }

    private System.Object[,] BuildObjectsArray() 
    {
        System.Object[,] arr = new System.Object[StatManagerRef.PropCount, 4];
        for(int i = 0 ; i < StatManagerRef.PropCount; i++) 
        {
            //Generate default?
            if (StatManagerRef.PropertiesToGenerate.Count <= i)
            {
                StatManagerRef.PropertiesToGenerate.Add(new StatManager.StatHandleObject("Default", 0f, 0f, 0f));
            }
            //Build array from stathandle objects
            var p = StatManagerRef.PropertiesToGenerate[i];
            arr[i, 0] = p.Name;
            arr[i, 1] = p.DefaultVal;
            arr[i, 2] = p.MinVal;
            arr[i, 3] = p.MaxVal;
        }
        return arr;
    }

    private void RecollectValues(System.Object[,] generatedData) 
    {
        for (int i = 0; i < StatManagerRef.PropertiesToGenerate.Count; i++)
        {
            //Build array from stathandle objects
            var p = StatManagerRef.PropertiesToGenerate[i];
            p.Name = (string)generatedData[i, 0];
            p.DefaultVal = (float)generatedData[i, 1];
            p.MinVal = (float)generatedData[i, 2];
            p.MaxVal = (float)generatedData[i, 3];
        }
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }



}
   
