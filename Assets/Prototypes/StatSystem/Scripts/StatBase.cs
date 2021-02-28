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

    private bool HasLimit;
    private float lowerLimit;
    private float upperLimit;


    public StatBase(string name) 
    {
        this.Name = name;
        Base = 0f;
        Multiplier = 1f;
        Increase = 1f;
        FlatAdded = 0f;
        FlatMultiplier = 1f;
    }

    public StatBase(string name, StatManager.StatHandleObject defaultVals)
    {
        this.Name = name;
        Base = defaultVals.DefaultVal;
        Multiplier = 1f;
        Increase = 1f;
        FlatAdded = 0f;
        FlatMultiplier = 1f;

        lowerLimit = defaultVals.MinVal;
        upperLimit = defaultVals.MaxVal;
        HasLimit = (lowerLimit != upperLimit) && (lowerLimit < upperLimit);
    }

    public float Get() 
    {
        return TurnacateValueByLimit( (((Base) * Increase) + (FlatAdded * FlatMultiplier)) * Multiplier);
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

    private float TurnacateValueByLimit(float val) 
    {
        if (HasLimit)
        {
            if (upperLimit < val)
            {
                val = upperLimit;
            }
            else if (lowerLimit > val) 
            {
                val = lowerLimit;
            }
        }
        return val;
    }

    public void SetFromSet(StatBaseValuesSet sbvs) 
    {
        this.Base = sbvs.Base;
        this.Multiplier = sbvs.Multiplier;
        this.Increase = sbvs.Increase;
        this.FlatAdded = sbvs.FlatAdded;
        this.FlatMultiplier = sbvs.FlatMultiplier;
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

    ///Used to transfer statbase values
    public struct StatBaseValuesSet
    {
        public float Base;
        public float Increase;
        public float Multiplier;
        public float FlatAdded;
        public float FlatMultiplier;

        public StatBaseValuesSet(float Base, float Increase, float Multiplier, float FlatAdded, float FlatMultiplier) 
        {
            this.Base = Base;
            this.Increase = Increase;
            this.Multiplier = Multiplier;
            this.FlatAdded = FlatAdded;
            this.FlatMultiplier = FlatMultiplier;
        }
    }


