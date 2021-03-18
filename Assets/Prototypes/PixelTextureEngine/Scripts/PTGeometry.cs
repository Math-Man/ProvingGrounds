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

    public static void line(PTCanvas canvas, int x1, int y1, int x2, int y2) 
    {

    }


}
