using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class SGEditorHelpers 
{
    /// <summary>
    /// Creates a dynamic, completly enclosed grid on the inspector.
    /// This doesn't make checks for matching typedata of the dynamic attached variables due to "dynamic" type checking being impossible.
    /// </summary>
    /// <param name="rowCount">Number of rows to generate</param>
    /// <param name="ColumnTypes">Typeof data for each column (ordered)</param>
    /// <param name="columnNames">String labels of columns (ordered)</param>
    /// <param name="vars">Variable to attain to a certain cell. Ordered in (column, row) array as in (x, y) top left being (0,0)</param>
    /// <returns>Modified data array</returns>
    public static System.Object[,] BuildPropGrid(int rowCount, List<System.Type> ColumnTypes, List<string> columnNames, System.Object[,] vars ) 
    {

        if(vars == null || ColumnTypes == null || columnNames == null)
            throw new Exception("Parameters cannot be null");

        if (ColumnTypes.Count != vars.GetLength(1))
            throw new Exception("Size mismatch between vars and columntypes");


        GUILayout.BeginHorizontal();
        for (int col = 0; col < columnNames.Count; col++)
        {
            GUILayout.BeginVertical();

            var type = ColumnTypes[col];
            var name = columnNames[col];

            if (!type.IsPrimitive && Type.GetTypeCode(type) != TypeCode.String) 
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                throw new Exception(type.ToString() + " Non primitive types apart from string cannot be used in prop grid.");
            }

            //Primary, headers
            GUILayout.Label(name);

            for (int row = 0; row < rowCount; row++) 
            {
                CreateEditorComponentOfType(type, row, col, vars);
            }

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        return vars;
    }


    private static void CreateEditorComponentOfType(System.Type type, int row, int col, System.Object[,] v) 
    {
        switch (Type.GetTypeCode(type)) 
        {
            case TypeCode.Int32:
                v[row, col] = EditorGUILayout.IntField((int)v[row, col]);
                break;
            case TypeCode.Single:
                v[row, col] = EditorGUILayout.FloatField((float)v[row, col]);
                break;
            case TypeCode.String:
                v[row, col] = EditorGUILayout.TextField((string)v[row, col]);
                break;
            case TypeCode.Int64:
                v[row, col] = EditorGUILayout.LongField((long)v[row, col]);
                break;
        }
    }



}
