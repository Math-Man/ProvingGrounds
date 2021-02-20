using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saveable : MonoBehaviour
{
    public virtual GenericObjectData data { get; set; }

    private void Awake()
    {
        data = new GenericObjectData();
    }

    private void Start()
    {
        init();
    }

    public virtual void init() 
    {
        data.transformData = new List<Vector3Ser>();
        Color mc = GetComponent<MeshRenderer>().sharedMaterial.color;
        data.materialColor = new float[4] { mc.r, mc.g, mc.b, mc.a };
    }

    public virtual void saveMe()
    {
        data.transformData.Add(new Vector3Ser(transform.position));
        data.transformData.Add(new Vector3Ser(transform.rotation.eulerAngles));
        data.transformData.Add(new Vector3Ser(transform.localScale));
        SceneSaveLoad.Instance.SaveSaveable(this);
    }




}
