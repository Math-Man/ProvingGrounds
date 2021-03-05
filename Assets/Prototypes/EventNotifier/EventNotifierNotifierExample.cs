using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNotifierNotifierExample : MonoBehaviour
{
    void Start()
    {
        ExampleEvent exEvent = new ExampleEvent();
        exEvent.Data = Random.Range(0, 100);
        EventNotifier.Notify(typeof(ExampleEvent), exEvent);
    }

}
