using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Grapher : MonoBehaviour
{

    [SerializeField] private int Resolution;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private float StepBerth;

    private LineRenderer lineRenderer;
    private float[] linePoints;
    

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        linePoints = new float[Resolution];

        float step = StepBerth * 2f / Resolution;
        var scale = Vector3.one * step;
        var pos = Vector3.zero;
        Vector3[] pointPositions = new Vector3[Resolution];
        for (int i = 0; i < Resolution; i++) 
        {
            linePoints[i] = (i + 0.5f) * step - 1f;
            pointPositions[i] = new Vector3(linePoints[i], 0, 0) + Offset;
        }

        lineRenderer.positionCount = Resolution;
        lineRenderer.SetPositions(pointPositions);

    }


    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < Resolution; i++)
        {
            float y = Mathf.Sin(Mathf.PI * (linePoints[i] / Mathf.PI + 0.5f * Time.time));
            y += 0.5f * Mathf.Sin(2f * Mathf.PI * (linePoints[i] / Mathf.PI + Time.time));


            lineRenderer.SetPosition(i, new Vector3(linePoints[i], y, 0) + Offset);

            //lineRenderer.SetPosition(i, new Vector3(linePoints[i],
            //    Mathf.Sin(linePoints[i] / Mathf.PI + Time.time) + Mathf.Sin(1.77f + linePoints[i] / Mathf.PI + Time.time),
            //    0) + Offset);
        }
    }
}
