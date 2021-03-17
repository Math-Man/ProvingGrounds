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
    [Header("Canvas Size Options")]
    [SerializeField] [Range(100, 16384)] private int TextureWidth;
    [SerializeField] [Range(100, 16384)] private int TextureHeight;
    [SerializeField] [Range(1, 16384)] private int PixelsScaling = 100;

    [Header("Scaling Options")]
    [Tooltip("Setting this will cause canvas width and height to be automaticly adjusted")] [SerializeField] bool autoScale;
    [Tooltip("Setting this will cause canvas width and height to be automaticly adjusted")] [SerializeField] bool fitToScreen;
    [Tooltip("Setting this will cause canvas width and height to be automaticly adjusted")] [SerializeField] bool FitWidth;
    [Tooltip("Setting this will cause canvas width and height to be automaticly adjusted")] [SerializeField] bool GenerateTexture;

    private Renderer ptRenderer;

    private PTCanvas canvas;
    private Camera MainCam;

    private void Awake()
    {
        MainCam = Camera.main;


        if (autoScale && !fitToScreen) 
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(TextureWidth / 100, TextureHeight / 100, 1));
        }

        if (fitToScreen && !autoScale) 
        {
            float quadHeight = MainCam.orthographicSize * 2.0f;
            float quadWidth = quadHeight * Screen.width / Screen.height;
            transform.localScale = new Vector3(quadWidth, quadHeight, 1);

            if(FitWidth)
                TextureWidth = Mathf.FloorToInt(TextureHeight * (quadWidth / quadHeight));
            else
                TextureHeight = Mathf.FloorToInt(TextureWidth * (quadHeight / quadWidth));
        }

        ptRenderer = GetComponent<Renderer>();

        if (GenerateTexture)
        {
            TextureWidth = TextureWidth * Mathf.FloorToInt((transform.localScale.x)) / 2;
            TextureHeight = TextureHeight * Mathf.FloorToInt((transform.localScale.y)) / 2;
            Texture2D texture = new Texture2D(TextureWidth, TextureHeight, TextureFormat.RGBA32, 0, true);
            texture.filterMode = FilterMode.Point;
            texture.mipMapBias = 0;
            ptRenderer.material.mainTexture = texture;
        }


        if (autoScale && PixelsScaling >= 100) 
        {
            TextureWidth /= (PixelsScaling / 100);
            TextureHeight /= (PixelsScaling / 100);
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
        //canvas.Background(Color.white);
        //AutoFillTest();
        //RandomFillTest();
        int mouseX, mouseY;
        Color col;

        PTGeometry.Stroke = Color.red;
        if (Input.GetMouseButton(0)) 
        {
            CursorInfo cursorInfo = PTUtil.CollectCursorInfo(MainCam);
            Debug.Log(cursorInfo.mouseUVX + "," + cursorInfo.mouseUVY + 
                " c: " + canvas.GetPixelColor(cursorInfo.mouseUVX, cursorInfo.mouseUVY));

            //PTUtil.CursorValueCast(canvas, MainCam, out mouseX, out mouseY, out col);
            //Debug.Log(mouseX + "," + mouseY + " color: " + col);
            PTGeometry.ellipse(canvas, cursorInfo.mouseUVX, cursorInfo.mouseUVY, 25, 25);
        }

        //PTGeometry.ellipse(canvas, 250, 500, 100, 50);
        canvas.Draw(ptRenderer, false);
        
    }

    int x = 0;
    int y = 0;
    private void AutoFillTest() 
    {
        if (y == TextureHeight - 1)
            return;
        canvas.SetPixel(new Color(
            (float)(x) % (float)(TextureWidth - 1) / 100 + 0.1f,
            (float)(y) % (float)(TextureHeight - 1)/ 33.3f + 0.1f,
            (float)((x/TextureWidth)+y) % (float)(Mathf.Sqrt(TextureWidth*TextureWidth + TextureHeight*TextureHeight) - 1)/100 + 0.1f), x, y);
        Debug.Log(x + ", " + y);
        if (x == TextureWidth - 1)
        {
            x = 0;
            y++;
        }
        else
            x++;
    }

    private void RandomFillTest() 
    {
        while (Random.Range(0, 3) <= 1)
            canvas.SetPixel(Random.ColorHSV(), Random.Range(0, TextureWidth - 1), Random.Range(0, TextureHeight - 1));
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
