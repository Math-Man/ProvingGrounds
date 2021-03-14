using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Dynamicly scales and handles individual pixels of a certain texture
/// </summary>
[RequireComponent(typeof(Renderer))]
public class PTEngine : MonoBehaviour
{
    [SerializeField] [Range(1, 4096)] private int TextureWidth;
    [SerializeField] [Range(1, 4096)] private int TextureHeight;
    [SerializeField] bool fitToScreen;
    [SerializeField] bool GenerateTexture;

    private Renderer ptRenderer;

    private PTCanvas canvas;
    private Camera MainCam;

    private void Awake()
    {
        MainCam = Camera.main;

        if (fitToScreen) 
        {
            float quadHeight = MainCam.orthographicSize * 2.0f;
            float quadWidth = quadHeight * Screen.width / Screen.height;
            transform.localScale = new Vector3(quadWidth, quadHeight, 1);

            TextureWidth = Mathf.FloorToInt(TextureHeight * (quadWidth / quadHeight));

        }




        ptRenderer = GetComponent<Renderer>();

        if (GenerateTexture)
        {
            Texture2D texture = new Texture2D(Mathf.FloorToInt(transform.localScale.x) * 100, Mathf.FloorToInt(transform.localScale.y) * 100);
            texture.filterMode = FilterMode.Point;
            ptRenderer.material.mainTexture = texture;
            

        }

        if (ptRenderer != null && ptRenderer.material.mainTexture != null)
        {
            ptRenderer.material.mainTexture.filterMode = FilterMode.Point;
            ptRenderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
            canvas = new PTCanvas(ptRenderer.material.mainTexture, TextureWidth, TextureHeight);
        }
        else if (ptRenderer.GetType() == typeof(SpriteRenderer)) 
        {
            canvas = new PTCanvas(ptRenderer.material.mainTexture, TextureWidth, TextureHeight);
        }


    }

    private void Update()
    {
        //canvas.SetPixel(Random.ColorHSV(), Random.Range(0, TextureWidth - 1), Random.Range(0, TextureHeight - 1));
        AutoFillTest();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", canvas.Draw(false));
        ptRenderer.SetPropertyBlock(block);

        
    }

    int x = 0;
    int y = 0;
    private void AutoFillTest() 
    {
        if (y == TextureHeight - 1)
            return;
        canvas.SetPixel(new Color( (float)(x) % (float)(TextureWidth - 1) / 100, 0, 0), x, y);
        Debug.Log(x + ", " + y);
        if (x == TextureWidth - 1)
        {
            x = 0;
            y++;
        }
        else
            x++;
    }

}

public class PTCanvas 
{
    public Texture2D ptTexture { get; private set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public List<CanvasPixel> canvasPixels { get; private set; }
    public List<CanvasPixel> canvasPixelsBuffer { get; private set; }
    public CanvasPixel[,] canvasIndexedPixelArray;
    public Color[] colorArray;


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

        //for (int i = 0; i < colorArray.Length; i++) 
        //{
        //    var pixel = new CanvasPixel(i, (i / width), (i % width), colorArray[i]);
        //    canvasPixels.Add(pixel);
        //    canvasIndexedPixelArray[(i % width), (i / width)] = pixel;
        //}

        RenderTexture.active = renderTexture;
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

    public Texture2D Draw(bool forceAll = false) 
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
        canvasIndexedPixelArray[x, y].col = color;
        canvasPixelsBuffer.Add(canvasIndexedPixelArray[x, y]);
    }

    public void SetPixel(Color color, CanvasPixel p)
    {
        p.col = color;
        canvasPixelsBuffer.Add(p);
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
}
