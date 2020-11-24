using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIObject : IDisposable {
    public bool isActive = true;
    //included 2d transform component
    public Vector2 position = Vector2.zero;
    public Quaternion rotation { get => Quaternion.Euler(0, 0, angle); }
    public Vector2 scale = Vector2.one;
    public Vector2 up { get => Vector2.up.Rotate(angle); }
    public float angle = 0;
    //part system
    private List<UIPart> parts = new List<UIPart>();
    public T GetPart<T>()
    {
        Type typeT = typeof(T);
        foreach (var p in parts)
            if (p.GetType() == typeT)
                return (T)Convert.ChangeType(p, typeT);
        return default(T);
    }
    public List<T> GetParts<T>()
    {
        List<T> results = new List<T>();
        Type typeT = typeof(T);
        if (typeT.IsInterface)
        {
            foreach (var p in parts)
            {
                var ints = p.GetType().GetInterfaces();
                foreach (var i in ints)
                    if (i == typeT)
                        results.Add((T)Convert.ChangeType(p, typeT));
            }
        }
        else
        {
            foreach (var p in parts)
                if (p.GetType() == typeT)
                    results.Add((T)Convert.ChangeType(p, typeT));
        }
        return results;
    }
    public UIObject AddPart<T>()
    {
        Type t = typeof(T);
        if (t.IsSubclassOf(UIPart.Type))
        {
            object instance = Activator.CreateInstance(t);
            (instance as UIPart).uiobj = this;
            parts.Add((UIPart)instance);
            return this;
        }
        else throw new NotSupportedException("Wrong Type");
    }
    public UIObject AddPart<T>(params object[] parms)
    {
        Type t = typeof(T);
        if (t.IsSubclassOf(UIPart.Type))
        {
            object instance = Activator.CreateInstance(t, parms);
            (instance as UIPart).uiobj = this;
            parts.Add((UIPart)instance);
            return this;
        }
        else throw new NotSupportedException("Wrong Type");
    }
    public UIObject AddPart(object instance)
    {
        Type t = instance.GetType();
        if (t.IsSubclassOf(UIPart.Type))
        {
            (instance as UIPart).uiobj = this;
            parts.Add((UIPart)instance);
            return this;
        }
        else throw new NotSupportedException("Wrong Type");
    }

    public void OnGUI()
    {
        foreach (var item in parts)
            if (item.PartActive)
                item.OnGUI();
    }
    public void KeyInput(KeyCode key)
    {
        foreach (var item in parts)
            if (item.PartActive)
                item.KeyInput(key);
    }
    public void Dispose()
    {
        foreach (var p in parts)
            p.Dispose();
    }
    //Static
    public static UIObject UICreate() => UICreate(new Rect(0, 0, 0, 0));
    public static UIObject UICreate(Rect rect)
    {
        UIObject obj = new UIObject();
        obj.position = rect.position;
        obj.scale = rect.size;
        Engine.UI_Objects.Add(obj);
        return obj;
    }
}
