using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedObjectScript : MonoBehaviour
{
    //Example script to make the object do stuff, hold data etc.

    [SerializeField] public float spin;

    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(transform.forward, spin);
    }
}
