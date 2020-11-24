using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIPart : IDisposable
{
    public bool PartActive = true;

    public UIObject uiobj;
    //links
    public Vector2 position { get => uiobj.position; set => uiobj.position = value; }
    public Quaternion rotation { get => Quaternion.Euler(0, 0, angle); }
    public Vector2 scale { get => uiobj.scale; set => uiobj.scale = value; }
    public float angle { get => uiobj.angle; set => uiobj.angle = value; }
    public T GetPart<T>()
    {
        if (uiobj == null) return default(T);
        return uiobj.GetPart<T>();
    }

    public virtual void OnGUI() { }
    public virtual void KeyInput(KeyCode key) { }
    public virtual void Dispose() { }

    public static System.Type Type = typeof(UIPart);

}