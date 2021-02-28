using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SBExample : MonoBehaviour
{
    public string statName = "Default";
    public float baseValue = 1f;
    public float increase = 1f;
    public float multiplier = 1f;
    public float flatAdded = 0f;
    public float flatMultiplier = 1f;

    void Start()
    {
        StatManager.Instance.RegisterObject(this);

    }

    public void Calculate() 
    {
        var statobj = StatManager.Instance.FindHandleObjectByName(statName);
        StatBase sBase = StatManager.Instance.GetStatBase(this, statName);

        if (sBase != null) 
        {
            StatManager.Instance.SetStat(this, statName, StatBase.Stat.BASE, baseValue);
            StatManager.Instance.SetStat(this, statName, StatBase.Stat.MULT, multiplier);
            StatManager.Instance.SetStat(this, statName, StatBase.Stat.FLAT, flatAdded);
            StatManager.Instance.SetStat(this, statName, StatBase.Stat.FLTMLT, flatMultiplier);
            StatManager.Instance.SetStat(this, statName, StatBase.Stat.INC, increase);

            var rs = StatManager.Instance.GetStat(this, statName);
            Debug.Log(rs);
        }

    }


}
