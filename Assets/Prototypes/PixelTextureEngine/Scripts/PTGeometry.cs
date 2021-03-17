using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PTGeometry
{
    public static Color Stroke { get; set; }
    public static Color Fill { get; set; }

    public static void rect(PTCanvas canvas, int x1, int y1, int x2, int y2) 
    {

    }

    /// <summary>
    /// Midpoint Ellipse algorithm
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="rx"></param>
    /// <param name="ry"></param>
    public static void ellipse(PTCanvas canvas, int xc, int yc, int rx, int ry) 
    {
        float x = 0f;
        float y = ry;
        float d1 = (rx * ry) - (rx * rx * ry) + (0.25f * rx * rx);
        float d2 = 0f;
        float dx = 2 * ry * ry * x;
        float dy = 2 * rx * rx * y;

        while (dx < dy)
        {
            canvas.SetPixel(Stroke, Mathf.FloorToInt(x + xc) , Mathf.FloorToInt(y + yc));
            canvas.SetPixel(Stroke, Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(y + yc));
            canvas.SetPixel(Stroke, Mathf.FloorToInt(x + xc), Mathf.FloorToInt(-y + yc));
            canvas.SetPixel(Stroke, Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(-y + yc));

            if (d1 < 0)
            {
                x++;
                dx = dx + (2 * ry * ry);
                d1 = d1 + dx + (ry * ry);
            }
            else
            {
                x++;
                y--;
                dx = dx + (2 * ry * ry);
                dy = dy - (2 * rx * rx);
                d1 = d1 + dx - dy + (ry * ry);
            }
        }

        d2 = ((ry * ry) * ((x + 0.5f) * (x + 0.5f)))
            + ((rx * rx) * ((y - 1) * (y - 1)))
            - (rx * rx * ry * ry);

        while (y >= 0)
        {

            canvas.SetPixel(Stroke, Mathf.FloorToInt(x + xc), Mathf.FloorToInt(y + yc));
            canvas.SetPixel(Stroke, Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(y + yc));
            canvas.SetPixel(Stroke, Mathf.FloorToInt(x + xc), Mathf.FloorToInt(-y + yc));
            canvas.SetPixel(Stroke, Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(-y + yc));

            if (d2 > 0)
            {
                y--;
                dy = dy - (2 * rx * rx);
                d2 = d2 + (rx * rx) - dy;
            }
            else
            {
                y--;
                x++;
                dx = dx + (2 * ry * ry);
                dy = dy - (2 * rx * rx);
                d2 = d2 + dx - dy + (rx * rx);
            }
        }

    }

    public static void line(PTCanvas canvas, int x1, int y1, int x2, int y2) 
    {

    }

}
