using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public sealed class StatBase 
{
    public string Name { get; set; }
    public float Base { get; set; }
    public float Increase { get; set; }
    public float Multiplier { get; set; }
    public float FlatAdded { get; set; }
    public float FlatMultiplier { get; set; }

    public StatBase(string name) 
    {
        this.Name = name;
        Base = 0f;
        Multiplier = 1f;
        Increase = 1f;
        FlatAdded = 0f;
        FlatMultiplier = 1f;
    }

    public float Get() 
    {
        return (Base + (FlatAdded * FlatMultiplier)) * Increase * Multiplier;
    }

    public void Set(Stat s, float val) 
    {
        switch (s) 
        {
            case Stat.BASE:
                Base = val;
                break;
            case Stat.FLAT:
                FlatAdded = val;
                break;
            case Stat.MULT:
                Multiplier = val;
                break;
            case Stat.INC:
                Increase = val;
                break;
            case Stat.FLTMLT:
                Increase = val;
                break;
        }
    }

    public enum Stat 
    {
        BASE,
        INC,
        MULT,
        FLAT,
        FLTMLT
    }

}


