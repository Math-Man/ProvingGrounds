using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

[System.Serializable]
public class GenericObjectData 
{
    public List<Vector3Ser> transformData;
    public float[] materialColor;
}

[System.Serializable]
public class SpiningCubeData : GenericObjectData 
{
    public float spin;
}


[System.Serializable]
public class Vector3Ser     
{
    public float a, b, c;

    public Vector3Ser(Vector3 v) 
    {
        Set(v);
    }

    public void Set(Vector3 v) 
    {
        a = v.x;
        b = v.y;
        c = v.z;
    }

    public Vector3 toVector3() 
    {
        return new Vector3(a, b, c);
    }
}