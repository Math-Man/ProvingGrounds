using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] public List<string> PropertiesToGenerate;

    public Dictionary<System.Object, Dictionary<string, StatBase>> BuiltObjectClasses { get; private set; }

    public static StatManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        BuiltObjectClasses = new Dictionary<object, Dictionary<string, StatBase>>();
    }

    public void RegisterObject(System.Object obj)
    {
        BuiltObjectClasses.Add(obj, new Dictionary<string, StatBase>());
        GenerateStateBaseObjects(obj);
    }

    public void GenerateStateBaseObjects(System.Object obj)
    {
        Dictionary<string, StatBase> objDict = BuiltObjectClasses[obj];
        foreach (string name in PropertiesToGenerate)
        {
            objDict.Add(name, new StatBase(name));
        }
    }

    public float GetStat(System.Object obj, string name)
    {
        try
        {
            return BuiltObjectClasses[obj][name].Get();
        }
        catch (KeyNotFoundException knf) 
        {
            Debug.LogError("No such stat: " + knf.Message);
            return 0f;
        }
    }

    public void SetStat(System.Object obj, string name, StatBase.Stat stat, float val)
    {
        try
        {
            BuiltObjectClasses[obj][name].Set(stat, val);
        }
        catch (KeyNotFoundException knf) 
        {
            Debug.LogError("No such stat: " + knf.Message);
        }
    }

    public void AddStat(System.Object obj, string name, StatBase.Stat stat, float val)
    {
        try
        {
            var v = BuiltObjectClasses[obj][name].Get() + val;
            BuiltObjectClasses[obj][name].Set(stat, v);
        }
        catch (KeyNotFoundException knf)
        {
            Debug.LogError("No such stat: " + knf.Message);
        }
    }

    public void MultStat(System.Object obj, string name, StatBase.Stat stat, float val)
    {
        try
        {
            var v = BuiltObjectClasses[obj][name].Get() * val;
            BuiltObjectClasses[obj][name].Set(stat, v);
        }
        catch (KeyNotFoundException knf)
        {
            Debug.LogError("No such stat: " + knf.Message);
        }
    }


}
