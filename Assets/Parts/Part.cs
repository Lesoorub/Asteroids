using UnityEngine;
using static UnityEngine.Mathf;

public abstract class Part : System.IDisposable
{
    public bool PartActive = true;

    public LogicalObject logicobj;

    public virtual void Start() { }
    public virtual void KeyInput() { }
    public virtual void Update() { }
    public virtual void OnGUI() { }
    public virtual void BtnUpdate(KeyCode key) { }
    public virtual void OnDrawGizmos() { }
    public virtual void Dispose() { }

    public static System.Type Type = typeof(Part);
    public void DrawCircle(Vector2 pos, float radius, float muliple = 6)
    {
        float points = 2 * PI * radius * muliple;
        Vector2 first = new Vector2(0, radius) + position;
        Vector2 last = first;
        for (float k = 0; k < points; k++)
        {
            Vector2 p = new Vector2(Sin(k / points * 2 * PI), Cos(k / points * 2 * PI)) * radius + position;
            Debug.DrawLine(last, p);
            last = p;
        }
        Debug.DrawLine(first, last);
    }
    //links
    public Vector2 position { get => logicobj.position; set => logicobj.position = value; }
    public Quaternion rotation { get => Quaternion.Euler(0, 0, angle); }
    public Vector2 scale { get => logicobj.scale; set => logicobj.scale = value; }
    public float angle { get => logicobj.angle; set => logicobj.angle = value; }
    public string tag { get => logicobj.tag; }
    public T GetPart<T>()
    {
        if (logicobj == null) return default(T);
        return logicobj.GetPart<T>();
    }
    public System.Collections.Generic.List<T> GetParts<T>()
    {
        if (logicobj == null) return new System.Collections.Generic.List<T>();
        return logicobj.GetParts<T>();
    }
    public void Destroy() => LogicalObject.Destroy(logicobj);

    public static void Destroy(LogicalObject obj) => LogicalObject.Destroy(obj);
    public static void Destroy(LogicalObject obj, float timer) => LogicalObject.Destroy(obj, timer);
}