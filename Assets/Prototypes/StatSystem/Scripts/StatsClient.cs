using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatsManager client component, not necessery to use but may make things easier
/// </summary>
public class StatsClient : MonoBehaviour
{
    public StatManager.StatHandleObject HandleObject { get; set; }

    private void Awake()
    {
        StatManager.Instance.RegisterObject(this);
    }



    public void ChangeValue(string name)
    {
      
    }

    public void GetValue()
    {

    }


    public enum StatName 
    {
       
    }


}
