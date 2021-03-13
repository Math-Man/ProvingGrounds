using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] public List<StatHandleObject> PropertiesToGenerate = new List<StatHandleObject>();
    [SerializeField] public int PropCount;

    public Dictionary<System.Object, Dictionary<string, StatBase>> BuiltObjectClasses { get; private set; }
    public static StatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("There can be only one statManager object loaded in the scene");
        Instance = this;
        BuiltObjectClasses = new Dictionary<object, Dictionary<string, StatBase>>();
    }

    public void RegisterObject(System.Object obj)
    {
        BuiltObjectClasses.Add(obj, new Dictionary<string, StatBase>());
        GenerateStatBaseObjects(obj);
    }

    public void RegisterObject(System.Object obj, Dictionary<string, StatBaseValuesSet> defaults) 
    {
        BuiltObjectClasses.Add(obj, new Dictionary<string, StatBase>());
        GenerateStatBaseObjects(obj, defaults);
    }

    public void GenerateStatBaseObjects(System.Object obj)
    {
        Dictionary<string, StatBase> objDict = BuiltObjectClasses[obj];
        foreach (StatHandleObject sho in PropertiesToGenerate)
        {
            var objName = sho.Name;
            objDict.Add(objName, new StatBase(objName, FindHandleObjectByName(objName)));
        }
    }

    public void GenerateStatBaseObjects(System.Object obj, Dictionary<string, StatBaseValuesSet> defaults)
    {
        Dictionary<string, StatBase> objDict = BuiltObjectClasses[obj];
        foreach (StatHandleObject sho in PropertiesToGenerate)
        {
            var objName = sho.Name;
            StatBase sb = new StatBase(objName, FindHandleObjectByName(objName));
            sb.SetFromSet(defaults[objName]);
            objDict.Add(objName, sb);
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

    public dynamic[] GetAllStats(System.Object obj) 
    {
        dynamic[] arr = new dynamic[BuiltObjectClasses[obj].Count];
        for (int i = 0; i < BuiltObjectClasses[obj].Count; i++) 
        {
            arr[i] = BuiltObjectClasses[obj][name].Get();
        }
        return arr;
    }

    public StatBase GetStatBase(System.Object obj, string name) 
    {
        return BuiltObjectClasses[obj][name];
    }

    public StatHandleObject FindHandleObjectByName(string name) 
    {
        return PropertiesToGenerate.First(sh => sh.Name == name);
    }

    [Serializable]
    public class StatHandleObject
    {
        public string Name;
        public float DefaultVal;
        public float MinVal;
        public float MaxVal;

        public StatHandleObject(string name, float defaultVal, float minVal, float maxVal)
        {
            Name = name;
            DefaultVal = defaultVal;
            MinVal = minVal;
            MaxVal = maxVal;
        }
    }


}
