using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatsManager client component, not necessery to use but may make things easier
/// </summary>
public class StatsClient : MonoBehaviour
{
    public StatManager ManagerObject { get; set; }
    public StatManager.StatHandleObject HandleObject { get; set; }

    private void Awake()
    {
        StatManager.Instance.RegisterObject(this);
        ManagerObject = StatManager.Instance;
    }



    public void ChangeValue(string name, StatBase.Stat stat, float value)
    {
        ManagerObject.SetStat(this, name, stat, value);
    }
        
    public void ChangeBase(string statName, float value) 
    {
        ChangeValue(statName, StatBase.Stat.BASE, value);
    }

    public void ChangeIncrease(string statName, float value)
    {
        ChangeValue(statName, StatBase.Stat.INC, value);
    }

    public void ChangeMultiplier(string statName, float value)
    {
        ChangeValue(statName, StatBase.Stat.MULT, value);
    }

    public void ChangeFlat(string statName, float value)
    {
        ChangeValue(statName, StatBase.Stat.FLAT, value);
    }

    public void ChangeFlatMult(string statName, float value)
    {
        ChangeValue(statName, StatBase.Stat.FLTMLT, value);
    }




    public float GetValue(string name)
    {
        return ManagerObject.GetStat(this, name);
    }

    public float GetBase(string statName) 
    {
        return ManagerObject.BuiltObjectClasses[this][statName].Base;
    }

    public float GetIncrease(string statName)
    {
        return ManagerObject.BuiltObjectClasses[this][statName].Increase;
    }

    public float GetMultiplier(string statName)
    {
        return ManagerObject.BuiltObjectClasses[this][statName].Multiplier;
    }

    public float GetFlat(string statName)
    {
        return ManagerObject.BuiltObjectClasses[this][statName].FlatAdded;
    }

    public float GetFlatMult(string statName)
    {
        return ManagerObject.BuiltObjectClasses[this][statName].FlatMultiplier;
    }



}
