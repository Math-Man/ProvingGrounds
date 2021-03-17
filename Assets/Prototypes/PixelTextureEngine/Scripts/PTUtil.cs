using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PTUtil 
{
    public static void DrawTextureImage() 
    {

    }

    public static void Clear() 
    {

    }

    public static CursorInfo CollectCursorInfo( Camera mainCam) 
    {
        RaycastHit hit;
        if (!Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
            return new CursorInfo();

        Renderer rend = hit.transform.GetComponent<Renderer>(); //use canvas to collect renderer
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return new CursorInfo();

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        Color c = tex.GetPixelBilinear(pixelUV.x/2, pixelUV.y/2);
        return new CursorInfo(pixelUV.x/2, pixelUV.y/2) ;

    }

}

public struct CursorInfo
{
    public float mouseUVXf, mouseUVYf;
    public int mouseUVX, mouseUVY;

    public CursorInfo(float muvxf, float muvyf) 
    {
        mouseUVXf = muvxf;
        mouseUVYf = muvyf;
        mouseUVX = Mathf.FloorToInt(mouseUVXf);
        mouseUVY = Mathf.FloorToInt(mouseUVYf);
    }

}
