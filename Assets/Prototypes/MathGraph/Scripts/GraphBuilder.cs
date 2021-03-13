using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Living up to my nickname 
public class GraphBuilder : MonoBehaviour
{
    [SerializeField] private GraphRendererType RendererType;
    [SerializeField] private bool useStaticFunction;
    [SerializeField] private List<float> FunctionValueChain;

    [SerializeField] private int RenderResolution;
    [SerializeField] private Vector3 RenderObjectOffset;
    [SerializeField] private float RenderStepBerth;

    public MathFunction mathFunc { get; set; }
    public GraphRenderer graphRend { get; set; }

    private void Start()
    {
        mathFunc = new MathFunction{ valueChain = FunctionValueChain.ToArray() };
        mathFunc.useStaticMethod = useStaticFunction;
        mathFunc.resolution = RenderResolution;

        switch (RendererType) 
        {
            case GraphRendererType.BOX:
                graphRend = new GraphBoxRenderer();
                break;
            case GraphRendererType.LINE:
                graphRend = new GraphLineRenderer();
                break;
        }

        //TODO: Move these to constructor
        graphRend.Resolution = RenderResolution;
        graphRend.Offset = RenderObjectOffset;
        graphRend.RenderStepBerth = RenderStepBerth;
        graphRend.BuildGameObject();

    }


    private void Update()
    {
        //Update values like this for now
        mathFunc.useStaticMethod = useStaticFunction;
        mathFunc.valueChain = FunctionValueChain.ToArray();
        mathFunc.resolution = RenderResolution;

        graphRend.Resolution = RenderResolution;
        graphRend.Offset = RenderObjectOffset;
        graphRend.RenderStepBerth = RenderStepBerth;


        graphRend.Render(mathFunc, Time.time);
    }

}

public enum GraphRendererType
{
    LINE, BOX
}


public class MathFunction 
{
    public bool useStaticMethod;
    public bool timeFactor;
    public int resolution;
    public float[] valueChain; // for {1, 2, 3} y = 1*x^0 + 2*x^1 + 3*x^3 ....

    public float Value(float x, float t)
    {
        if (useStaticMethod)
        {
            //Write a static method here example method:
            var sm = Mathf.Sin(Mathf.PI * (x/resolution + (t)));
                
            return sm;

        }
        else
        {
            float sm = 0;
            for (int i = 0; i < valueChain.Length; i++)
            {
                //if (timeFactor)
                    //sm += Mathf.Pow(valueChain[i] * (x + (Mathf.Sin(Mathf.PI * t) * resolution/2f) ), i);
                //else
                sm += Mathf.Pow(valueChain[i] * x, i);
            }
            return sm;
        }

    }

}

