using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBExample : MonoBehaviour
{
    void Start()
    {

        StatManager.Instance.RegisterObject(this);


        var statName = StatManager.Instance.PropertiesToGenerate[0];

        StatManager.Instance.SetStat(this, statName, StatBase.Stat.BASE, 10f);
        StatManager.Instance.SetStat(this, statName, StatBase.Stat.INC, 1.2f);
        StatManager.Instance.SetStat(this, statName, StatBase.Stat.FLAT, 16f);
        StatManager.Instance.SetStat(this, statName, StatBase.Stat.FLTMLT, 2f);


        Debug.Log(StatManager.Instance.GetStat(this, statName));
    }

}
