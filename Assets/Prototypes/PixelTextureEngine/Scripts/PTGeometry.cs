using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PTGeometry
{
    public static Color Stroke { get; set; }
    public static Color Fill { get; set; }
    public static int StrokeWidth { get; set; }

    public static void rect(PTCanvas canvas, int x1, int y1, int x2, int y2) 
    {
        for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2); y++) 
        {
            for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2); x++)
            {
                if (x > Math.Min(x1, x2) + StrokeWidth &&
                   y > Math.Min(y1, y2) + StrokeWidth &&
                   x < Math.Max(x1, x2) - StrokeWidth &&
                   y < Math.Max(y1, y2) - StrokeWidth)
                    canvas.SetPixel(Fill, x, y);
                else
                    canvas.SetPixel(Stroke, x, y);
            }
        }
    }

    /// <summary>
    /// Midpoint Ellipse algorithm
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="rx"></param>
    /// <param name="ry"></param>
    public static void ellipse(PTCanvas canvas, int xc, int yc, int rx, int ry, bool fill = false) 
    {
        float x = 0f;
        float y = ry;
        float d1 = (rx * ry) - (rx * rx * ry) + (0.25f * rx * rx);
        float d2 = 0f;
        float dx = 2 * ry * ry * x;
        float dy = 2 * rx * rx * y;

        int strokeWidthCalc = Mathf.CeilToInt(StrokeWidth / 2);
        rx += strokeWidthCalc;
        ry += strokeWidthCalc;


        while (dx < dy)
        {

            int[] point1 = new int[] { Mathf.FloorToInt(x + xc),  Mathf.FloorToInt(y + yc) };  //top right
            int[] point2 = new int[] { Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(y + yc) };  //top left
            int[] point3 = new int[] { Mathf.FloorToInt(x + xc),  Mathf.FloorToInt(-y + yc) }; //bot right
            int[] point4 = new int[] { Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(-y + yc) }; //bot left

            for (int i = -strokeWidthCalc - 1; i <= strokeWidthCalc; i++)
            {
                canvas.SetPixel(Stroke, point1[0] + i, point1[1] + i);
                canvas.SetPixel(Stroke, point2[0] - i, point2[1] + i);
                canvas.SetPixel(Stroke, point3[0] + i, point3[1] - i);
                canvas.SetPixel(Stroke, point4[0] - i, point4[1] - i);
            }

            if (fill)
            {
                for (int i = point2[0] + strokeWidthCalc; i <= xc; i++) //top left
                    canvas.SetPixel(Fill, i, point2[1]);
                for (int i = point1[0] - strokeWidthCalc; i >= xc; i--) //top right
                    canvas.SetPixel(Fill, i, point1[1]);

                for (int i = point4[0] + strokeWidthCalc; i <= xc; i++) //bot left
                    canvas.SetPixel(Fill, i, point4[1]);
                for (int i = point3[0] - strokeWidthCalc; i >= xc; i--) //bot right
                    canvas.SetPixel(Fill, i, point3[1]);
            }

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

            int[] point1 = new int[] { Mathf.FloorToInt(x + xc), Mathf.FloorToInt(y + yc) };  //top right
            int[] point2 = new int[] { Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(y + yc) };  //top left
            int[] point3 = new int[] { Mathf.FloorToInt(x + xc), Mathf.FloorToInt(-y + yc) }; //bot right
            int[] point4 = new int[] { Mathf.FloorToInt(-x + xc), Mathf.FloorToInt(-y + yc) }; //bot left

            for (int i = -strokeWidthCalc - 1; i <= strokeWidthCalc; i++)
            {
                canvas.SetPixel(Stroke, point1[0] + i, point1[1] + i);
                canvas.SetPixel(Stroke, point2[0] - i, point2[1] + i);
                canvas.SetPixel(Stroke, point3[0] + i, point3[1] - i);
                canvas.SetPixel(Stroke, point4[0] - i, point4[1] - i);
            }

            if (fill)
            {
                for (int i = point2[0] + strokeWidthCalc; i <= xc; i++) //top left
                    canvas.SetPixel(Fill, i, point2[1]);
                for (int i = point1[0] - strokeWidthCalc; i >= xc; i--) //top right
                    canvas.SetPixel(Fill, i, point1[1]);

                for (int i = point4[0] + strokeWidthCalc; i <= xc; i++) //bot left
                    canvas.SetPixel(Fill, i, point4[1]);
                for (int i = point3[0] - strokeWidthCalc; i >= xc; i--) //bot right
                    canvas.SetPixel(Fill, i, point3[1]);
            }


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

    /*http://members.chello.at/~easyfilter/bresenham.html*/
    public static void plotLineWidth(PTCanvas canvas, int x0, int y0, int x1, int y1, float wd)
    {
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx - dy, e2, x2, y2;                          
        float ed = dx + dy == 0 ? 1 : Mathf.Sqrt((float)dx * dx + (float)dy * dy);

        for (wd = (wd + 1) / 2; ;)
        {                                  
            canvas.SetPixel(Stroke, x0, y0);
            e2 = err; x2 = x0;
            if (2 * e2 >= -dx)
            {                                         
                for (e2 += dy, y2 = y0; e2 < ed * wd && (y1 != y2 || dx > dy); e2 += dx)
                    canvas.SetPixel(Stroke, x0, y2 += sy);
                if (x0 == x1) break;
                e2 = err; err -= dy; x0 += sx;
            }
            if (2 * e2 <= dy)
            {                                          
                for (e2 = dx - e2; e2 < ed * wd && (x1 != x2 || dx < dy); e2 += dy)
                    canvas.SetPixel(Stroke, x2 += sx, y0);
                if (y0 == y1) break;
                err += dx; y0 += sy;
            }
        }
    }



}
