using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.VFX;

public class GatherEffectTargeter : MonoBehaviour
{
    private VisualEffect GatherEffect;
    public Transform Target;


    private string CONE_HEIGHT = "ConeHeight";
    private string CONE_OUTER_RADI = "ConeOuterRadius";
    private string KILL_SP_POS = "IN_TARGET_POS";
    private string TARG_DIST = "IN_TARGET_DISTANCE";


    private void Awake()
    {
        GatherEffect = GetComponent<VisualEffect>();
    }

    void Start()
    {

    }

    void Update()
    {
        UpdateTargeter();
    }

    private void UpdateTargeter() 
    {
        GatherEffect.SetVector3(KILL_SP_POS, Target != null ? Target.position : Vector3.zero);
        GatherEffect.SetFloat(TARG_DIST, Target != null ? Vector3.Distance(Target.position, transform.position) : 0f);
        
        Target = SearchTarget();
        LockOnTarget();

        //Lose target
        if (Vector3.Distance(Target.position, transform.position) > 10000f) 
        {
            Target = null;
        }
    }

    private void LockOnTarget() 
    {
        if (Target != null) 
        {
            transform.LookAt(Target);
            transform.Rotate(90, 0, 0);
        }
        GatherEffect.SetFloat(CONE_HEIGHT, Target != null ?
            Vector3.Distance(Target.position, transform.position): 0);
    }

    private Transform SearchTarget() 
    {
        var trgt = STHelper.GetCollidersInRangeByTag(Vector3.zero, 10000f, "GATHER_TARGET").FirstOrDefault();
        if (trgt != null)
            return trgt.transform;
        else
            return null;
    }
    

}
