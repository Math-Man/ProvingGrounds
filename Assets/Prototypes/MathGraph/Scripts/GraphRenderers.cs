using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphRenderer
{
    public GameObject rendererGameObject;
    public GraphRendererType RenderType;
    public int Resolution; //TODO: Make these protected
    public Vector3 Offset;
    public float RenderStepBerth;

    public virtual void BuildGameObject() 
    {
        throw new NotImplementedException();
    }

    public virtual void Render(MathFunction func, float tx)
    {
        throw new NotImplementedException();
    }

}


public class GraphBoxRenderer : GraphRenderer
{
    private List<GameObject> pointObjects;

    public GraphBoxRenderer()
    {
        RenderType = GraphRendererType.BOX;
        pointObjects = new List<GameObject>();
    }
    
    public override void BuildGameObject()
    {
        rendererGameObject = new GameObject();
        rendererGameObject.transform.position += Offset;

        float step = 2f / Resolution;
        var scale = Vector3.one * step;
        var pos = Vector3.zero;

        for (int i = 0; i < Resolution; i++) 
        {
            var b = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Transform pn = b.transform;
            pn.SetParent(rendererGameObject.transform);

            pos.x = (i + 0.5f) * step - 1f;
            pn.localPosition = pos;

            pn.localScale = scale;
            pointObjects.Add(b);
        }
    }

    public override void Render(MathFunction func, float tx)
    {
        for (int i = 0; i < Resolution; i++)
        {
            var pos = pointObjects[i].transform.position;
            pos.y = func.Value(pos.x / (2f/Resolution), tx) * RenderStepBerth;

            pointObjects[i].transform.position = pos;
        }
    }

}

public class GraphLineRenderer : GraphRenderer
{
    public LineRenderer lr;

    public GraphLineRenderer() 
    {
        RenderType = GraphRendererType.LINE;
    }
    public override void BuildGameObject()
    {
        rendererGameObject = new GameObject();
        rendererGameObject.transform.position += Offset;
        
        lr = rendererGameObject.AddComponent<LineRenderer>();
        lr.positionCount = Resolution;

        float step = 2f / Resolution;
        var scale = Vector3.one * step;
        var pos = Vector3.zero;

        lr.startWidth = step;
        lr.endWidth = step;

        for (int i = 0; i < Resolution; i++)
        {
            pos.x = (i + 0.5f) * step - 1f;
            lr.SetPosition(i, pos);
        }

    }

    public override void Render(MathFunction func, float tx) 
    {
        for (int i = 0; i < Resolution; i++)
        {
            var pos = lr.GetPosition(i);
            pos.y = func.Value(pos.x / (2f / Resolution), tx) * RenderStepBerth;

            lr.SetPosition(i, pos);
        }
    }

}

