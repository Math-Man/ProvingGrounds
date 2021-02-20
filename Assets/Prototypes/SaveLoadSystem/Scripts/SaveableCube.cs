using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableCube : Saveable
{

    public override GenericObjectData data { get; set; }

    private void Awake()
    {
        data = new SpiningCubeData();
    }

    private void Start()
    {
        init();
    }

    public override void saveMe()
    {
        data.transformData.Add(new Vector3Ser(transform.position));
        data.transformData.Add(new Vector3Ser(transform.rotation.eulerAngles));
        data.transformData.Add(new Vector3Ser(transform.localScale));
        ((SpiningCubeData)data).spin = GetComponent<SavedObjectScript>().spin;
        
        SceneSaveLoad.Instance.SaveSaveable(this);

    }

}
