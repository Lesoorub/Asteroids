using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalObject : IDisposable
{
    public bool isActive = true;
    //links
    private FastPhysics fastPhysics;
    public FastPhysics FastPhysics { get { if (fastPhysics == null) fastPhysics = GetPart<FastPhysics>(); return fastPhysics; } set => fastPhysics = value; }
    private PolyRenderer polyRenderer;
    public PolyRenderer PolyRenderer { get { if (polyRenderer == null) polyRenderer = GetPart<PolyRenderer>(); return polyRenderer; } set => polyRenderer = value; }
    private SpriteDrawer spriteDrawer;
    public SpriteDrawer SpriteDrawer { get { if (spriteDrawer == null) spriteDrawer = GetPart<SpriteDrawer>(); return spriteDrawer; } set => spriteDrawer = value; }

    //included 2d transform component
    public Vector2 position = Vector2.zero;
    public Quaternion rotation { get => Quaternion.Euler(0, 0, angle); }
    public Vector2 scale = Vector2.one;
    public Vector2 up { get => Vector2.up.Rotate(angle); }
    public float angle = 0;
    //tags
    public List<string> tags = new List<string>();
    public string tag {
        get => (tags.Count > 0) ? tags[0] : "Untagged";
        set { if (tags.Count > 0) tags[0] = value; else tags.Add(value); }
    }
    //External Code
    private Dictionary<string, Action> acts = new Dictionary<string, Action>();
    public void AddScript(string name, Action act) => acts.Add(name, act);
    public void RemoveScript(string name) => acts.Remove(name);
    //Part System
    private List<Part> parts = new List<Part>();
    public T GetPart<T>()
    {
        Type typeT = typeof(T);
        foreach (var p in parts)
            if (p.GetType() == typeT)
                return (T)(object)p;
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
                        results.Add((T)(object)p);
            }
        }
        else
        {
            foreach (var p in parts)
                if (p.GetType() == typeT)
                    results.Add((T)(object)p);
        }
        return results;
    }
    public LogicalObject AddPart<T>()
    {
        Type t = typeof(T);
        if (t.IsSubclassOf(Part.Type))
        {
            object instance = Activator.CreateInstance(t);
            (instance as Part).logicobj = this;
            parts.Add((Part)instance);
            return this;
        }
        else throw new NotSupportedException("Wrong Type");
    }
    public LogicalObject AddPart<T>(params object[] parms)
    {
        Type t = typeof(T);
        if (t.IsSubclassOf(Part.Type))
        {
            object instance = Activator.CreateInstance(t, parms);
            (instance as Part).logicobj = this;
            parts.Add((Part)instance);
            return this;
        }
        else throw new NotSupportedException("Wrong Type");
    }
    public LogicalObject AddPart(object instance)
    {
        Type t = instance.GetType();
        if (t.IsSubclassOf(Part.Type))
        {
            (instance as Part).logicobj = this;
            parts.Add((Part)instance);
            return this;
        }
        else throw new NotSupportedException("Wrong Type");
    }
    //Methods
    public void Start()
    {
        foreach (var item in parts)
            item.Start();
    }
    public void KeyInput(KeyCode key)
    {
        foreach (var item in parts)
            if (item.PartActive)
                item.KeyInput(key);
    }
    public void Update()
    {
        foreach (var item in parts)
            if (item.PartActive)
            {
                item.OnDrawGizmos();
                item.Update();
            }
        foreach (var a in acts) a.Value();
    }
    public void Dispose()
    {
        foreach (var p in parts)
            p.Dispose();
        tags.Clear();
        acts.Clear();
    }

    //Static field
    public static LogicalObject Create(string tag = "Untagged") => Create(Vector2.zero, 0, Vector2.one, tag);
    public static LogicalObject Create(Vector2 position, string tag = "Untagged") => Create(position, 0, Vector2.one, tag);
    public static LogicalObject Create(float angle, string tag = "Untagged") => Create(Vector2.zero, angle, Vector2.one, tag);
    public static LogicalObject Create(Vector2 position, float angle, string tag = "Untagged") => Create(position, angle, Vector2.one, tag);
    public static LogicalObject Create(Vector2 position, Vector2 scale, string tag = "Untagged") => Create(position, 0, scale, tag);
    public static LogicalObject Create(Vector2 position, float angle, Vector2 scale, string tag = "Untagged")
    {
        LogicalObject obj = new LogicalObject();
        obj.tag = tag;
        obj.position = position;
        obj.angle = angle;
        obj.scale = scale;
        Engine.isnew.Enqueue(obj);
        return obj;
    }
    public static LogicalObject Create(Vector2 position, float angle, Vector2 scale, List<string> tags)
    {
        LogicalObject obj = new LogicalObject();
        obj.tags = tags;
        obj.position = position;
        obj.angle = angle;
        obj.scale = scale;
        Engine.isnew.Enqueue(obj);
        return obj;
    }
    public static LogicalObject Create(List<string> tags)
    {
        LogicalObject obj = new LogicalObject();
        obj.tags = tags;
        Engine.isnew.Enqueue(obj);
        return obj;
    }

    public static void Destroy(LogicalObject obj)
    {
        Engine.Objects.Remove(obj);
    }
    public static void Destroy(LogicalObject obj, float lifetime)
    {
        Engine.StartCoroutine(DestroyTimer(obj, lifetime));
    }
    public static IEnumerator DestroyTimer(LogicalObject obj, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Engine.Objects.Remove(obj);
        yield return null;
    }
}