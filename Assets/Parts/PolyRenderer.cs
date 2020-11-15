#region usings
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion usings

[ExecuteAlways]
public class PolyRenderer : Part
{
    public Vector2[] Points;
    public Color color = Color.white;
    public bool Loop = true;

    public PolyRenderer()
    {
        Points = new Vector2[0];
    }
    public PolyRenderer(Vector2[] points)
    {
        Points = points;
    }
    public PolyRenderer(Vector2[] points, Color color)
    {
        Points = points;
        this.color = color;
    }

    public override void Update()
    {
        GL.PushMatrix();
        Chache.stdmaterial.SetPass(0);
        GL.MultMatrix(Matrix4x4.TRS(position, rotation, scale));
        
        GL.Begin(GL.LINES);
        GL.Color(color);
        if (Points.Length != 0)
        {
            Vector2 last = Points[0];
            foreach (var p in Points)
            {
                GL.Vertex(last);
                GL.Vertex(p);
                last = p;
            }
            if (Loop)
            {
                GL.Vertex(last);
                GL.Vertex(Points[0]);
            }
        }
        GL.End();

        GL.PopMatrix();
    }
#if DEBUG
    public override void OnDrawGizmos()
    {
        var m = Matrix4x4.TRS(position, rotation, scale);
        if (!PartActive) return;
        if (Points.Length != 0)
        {
            Vector2 last = m.MultiplyPoint(Points[0]);
            foreach (var p in Points)
            {
                var t = m.MultiplyPoint(p);
                Debug.DrawLine(last, t, color);
                last = t;
            }
            Debug.DrawLine(last, m.MultiplyPoint(Points[0]), color);
        }
    }
#endif
}