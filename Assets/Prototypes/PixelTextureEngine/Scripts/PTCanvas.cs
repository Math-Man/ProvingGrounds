using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PTCanvas
{
    public Texture2D ptTexture { get; private set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public List<CanvasPixel> canvasPixels { get; private set; }
    public List<CanvasPixel> canvasPixelsBuffer { get; private set; }
    public CanvasPixel[,] canvasIndexedPixelArray;
    public Color[] colorArray;

    private MaterialPropertyBlock block;

    public PTCanvas(Texture tex, int width, int height)
    {
        canvasPixels = new List<CanvasPixel>();
        canvasPixelsBuffer = new List<CanvasPixel>();
        canvasIndexedPixelArray = new CanvasPixel[width, height];

        Width = width;
        Height = height;
        ptTexture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
        RenderTexture renderTexture = new RenderTexture(Width, Height, 32);
        Graphics.Blit(tex, renderTexture);
        RenderTexture.active = renderTexture;
        ptTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        ptTexture.Apply();


        colorArray = ptTexture.GetPixels();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pixel = new CanvasPixel(x + y, x, y, colorArray[x + y]);
                canvasPixels.Add(pixel);
                canvasIndexedPixelArray[x, y] = pixel;
            }
        }

        RenderTexture.active = renderTexture;

        block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", ptTexture);

    }

    private void UpdateCanvasTexture()
    {
        UpdateColorsArray();
        canvasPixelsBuffer.Clear();
    }

    private void UpdateCanvasTexturePerPixelBasis()
    {
        foreach (CanvasPixel pixel in canvasPixelsBuffer)
        {
            ptTexture.SetPixel(pixel.x, pixel.y, pixel.col);
        }
        canvasPixelsBuffer.Clear();
    }

    public Texture2D Draw(Renderer ptRenderer, bool forceAll = false)
    {
        if (forceAll)
        {
            UpdateCanvasTexture();
            ptTexture.SetPixels(colorArray);
        }
        else
        {
            UpdateCanvasTexturePerPixelBasis();
        }
        ptTexture.Apply();
        ptRenderer.SetPropertyBlock(block);
        return ptTexture;
    }


    public void UpdateColorsArray()
    {
        Parallel.ForEach<CanvasPixel>(canvasPixelsBuffer, pixel =>
        {
            colorArray[pixel.index] = pixel.col;
        });
    }

    public void Background(Color color)
    {
        Parallel.ForEach<CanvasPixel>(canvasPixels, pixel =>
        {
            SetPixel(color, pixel);
        });
    }

    public void SetPixel(Color color, int x, int y)
    {
        //var fp = canvasPixels.Find(cp => cp.x == x && cp.y == y);
        try
        {
            canvasIndexedPixelArray[x, y].col = color;
            canvasPixelsBuffer.Add(canvasIndexedPixelArray[x, y]);
        }
        catch (System.IndexOutOfRangeException iofre)
        {

        }
    }

    public void SetPixel(Color color, CanvasPixel p)
    {
        p.col = color;
        canvasPixelsBuffer.Add(p);
    }

    public Color GetPixelColor(int x, int y)
    {
        return ptTexture.GetPixel(x, y);
    }

    public Color GetPixelColorBilliner(float x, float y)
    {
        return ptTexture.GetPixelBilinear(x, y);
    }

}

public struct CanvasPixel
{
    public int index;
    public int x;
    public int y;
    public Color col;
    public CanvasPixel(int index, int x, int y, Color col)
    {
        this.index = index;
        this.x = x;
        this.y = y;
        this.col = col;
    }
}

