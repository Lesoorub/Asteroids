using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalObject : IDisposable
{
    public bool isActive = true;
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
    private List<KeyValuePair<Type, object>> parts = new List<KeyValuePair<Type, object>>();
    public T GetPart<T>()
    {
        Type typeT = typeof(T);
        foreach (var p in parts)
            if (p.Key == typeT)
                return (T)p.Value;
        return default(T);
    }
    public List<T> GetParts<T>()
    {
        List<T> results = new List<T>();
        Type typeT = typeof(T);
        if (typeT.IsInterface)
        {
            //typeof(MyType).GetInterfaces().Any(i => i.IsGenericType &
            //& i.GetGenericTypeDefinition() == typeof(IMyInterface<>))
            foreach (var p in parts)
            {
                var ints = p.Key.GetInterfaces();
                foreach (var i in ints)
                    if (i == typeT)
                        results.Add((T)p.Value);
            }
        }
        else
        {
            foreach (var p in parts)
                if (p.Key == typeT)
                    results.Add((T)p.Value);
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
            parts.Add(new KeyValuePair<Type, object>(t, instance));
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
            parts.Add(new KeyValuePair<Type, object>(t, instance));
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
            parts.Add(new KeyValuePair<Type, object>(t, instance));
            return this;
        }
        else throw new NotSupportedException("Wrong Type");
    }
    //Methods
    public void Start()
    {
        foreach (var item in parts)
        {
            Part p = item.Value as Part;
            p.Start();
        }
    }
    public void KeyInput()
    {
        foreach (var item in parts)
        {
            Part p = item.Value as Part;
            if (p.PartActive)
                p.KeyInput();
        }
    }
    public void Update()
    {
        foreach (var item in parts)
        {
            Part p = item.Value as Part;
            if (p.PartActive)
            {
                p.OnDrawGizmos();
                p.Update();
            }
        }
        foreach (var a in acts) a.Value();
    }
    public void OnGUI()
    {
        foreach (var item in parts)
        {
            Part p = item.Value as Part;
            if (p.PartActive)
                p.OnGUI();
        }
    }
    public void BtnClick(KeyCode key)
    {
        foreach (var item in parts)
        {
            Part p = item.Value as Part;
            if (p.PartActive)
                p.BtnUpdate(key);
        }
    }
    public void Dispose()
    {
        foreach (var p in parts)
            (p.Value as Part).Dispose();
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
        Engine.addq.Enqueue(obj);
        return obj;
    }
    public static LogicalObject Create(Vector2 position, float angle, Vector2 scale, List<string> tags)
    {
        LogicalObject obj = new LogicalObject();
        obj.tags = tags;
        obj.position = position;
        obj.angle = angle;
        obj.scale = scale;
        Engine.addq.Enqueue(obj);
        return obj;
    }
    public static LogicalObject Create(List<string> tags)
    {
        LogicalObject obj = new LogicalObject();
        obj.tags = tags;
        Engine.addq.Enqueue(obj);
        return obj;
    }

    public static LogicalObject UICreate(string tag = "Untagged") => UICreate(new Rect(0,0,0,0), tag);
    public static LogicalObject UICreate(Rect rect, string tag = "Untagged")
    {
        LogicalObject obj = new LogicalObject();
        obj.tag = tag;
        obj.position = rect.position;
        obj.scale = rect.size;
        Engine.UI_Objects.Add(obj);
        return obj;
    }

    public static void Destroy(LogicalObject obj)
    {
        Engine.delq.Enqueue(obj);
    }
    public static void Destroy(LogicalObject obj, float lifetime)
    {
        Engine.StartCoroutine(DestroyTimer(obj, lifetime));
    }
    public static IEnumerator DestroyTimer(LogicalObject obj, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Engine.delq.Enqueue(obj);
        yield return null;
    }
}