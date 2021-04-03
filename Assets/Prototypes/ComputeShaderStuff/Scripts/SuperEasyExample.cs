using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuperEasyExample : MonoBehaviour
{

    public ComputeShader shader;
    [Range(1, 100000000)]public int batchSize = 100; //Modify the [numthreads(64,1,1)] line in SuperEasyExample.compute to check how speed changes
    public int ThreadPool = 128; //Must be mult of 2, max 65535, min 1
    public bool useShader;
    public bool ExecuteBoth;

    private void Awake()
    {
        var dataset = CreateDataSet();

        System.Diagnostics.Stopwatch stopWatch;
        

        if (useShader || ExecuteBoth) 
        {
            stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            RunShader(dataset);
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}..{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Ticks);
            Debug.Log("SHADER RunTime " + elapsedTime);
        }

        if (!useShader || ExecuteBoth)
        {
            stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            NumPair[] output = new NumPair[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                //Write some really hard math here
                float resultOfHardMath = Mathf.Tan(Mathf.Sin(dataset[i].p1) + Mathf.Sin(dataset[i].p2)); //this isnt that hard :/
                output[i] = new NumPair(resultOfHardMath, resultOfHardMath);
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}..{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Ticks);
            Debug.Log("CPU RunTime " + elapsedTime);
        }

    }

    struct NumPair 
    {
        public float p1;
        public float p2;
        public NumPair(float p1, float p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }

    private NumPair[] CreateDataSet() 
    {
        NumPair[] data = new NumPair[batchSize];

        for (int i = 0; i < batchSize; i++)
        {
            data[i] = new NumPair(UnityEngine.Random.Range(-100000, 100000), UnityEngine.Random.Range(-100000, 100000));
        }
        
        return data;
    }

    void RunShader(NumPair[] dataset)
    {
        NumPair[] data = dataset;

        //Size of a float is 4 bytes (each struct has 2 floats)
        int bufferSize = 4 * 2;

        //Call compute buffer
        ComputeBuffer buffer = new ComputeBuffer(data.Length, bufferSize);
        buffer.SetData(data);
        int kernel = shader.FindKernel("Calculate");
        shader.SetBuffer(kernel, "dataBuffer", buffer);
        shader.Dispatch(kernel, ThreadPool, 1, 1);

        //Get output with a block of same size and dispose the buffer
        NumPair[] output = new NumPair[batchSize];
        buffer.GetData(output);
        buffer.Dispose();

    }




}
