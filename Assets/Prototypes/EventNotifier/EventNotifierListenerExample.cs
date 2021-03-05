using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNotifierListenerExample : MonoBehaviour
{
    public void Awake()
    {
        EventNotifier.RegisterNotification(typeof(ExampleEvent), ExampleCallback);
    }

    public void ExampleCallback(BaseEvent obj) 
    {
        if (obj is ExampleEvent)
        {
            var exObj = (ExampleEvent)obj;
            Debug.Log(exObj.Data);
        }
    }

    private void OnDestroy()
    {
        EventNotifier.UnRegisterNotification(typeof(ExampleEvent), ExampleCallback);
    }

}


public class ExampleEvent : BaseEvent
{
    public int Data;
}