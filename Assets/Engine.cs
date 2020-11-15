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

public class Engine : MonoBehaviour
{
    public static Engine current;
    public static Queue<LogicalObject> addq = new Queue<LogicalObject>();
    public static Queue<LogicalObject> delq = new Queue<LogicalObject>();
    public static List<LogicalObject> Objects = new List<LogicalObject>();
    public static List<LogicalObject> UI_Objects = new List<LogicalObject>();
    void Start()
    {
        current = this;
        //start
        AsteroidsGame.Start();
        //
        PushObjects();
        foreach (var o in Objects)
            o.Start();
    }

    void OnEnable() =>
        Camera.onPostRender += MyPostRenderer;
    void OnDisable() =>
        Camera.onPostRender -= MyPostRenderer;

    private void Update()
    {
        foreach (var o in Objects)
            if (o.isActive)
                o.KeyInput();
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                foreach (var o in UI_Objects)
                    if (o.isActive)
                        o.BtnClick(KeyCode.Mouse0);
            if (Input.GetKeyDown(KeyCode.Mouse1))
                foreach (var o in UI_Objects)
                    if (o.isActive)
                        o.BtnClick(KeyCode.Mouse1);
            if (Input.GetKeyDown(KeyCode.Mouse2))
                foreach (var o in UI_Objects)
                    if (o.isActive)
                        o.BtnClick(KeyCode.Mouse2);
        }
    }
    private void MyPostRenderer(Camera cam)
    {
        PushObjects();
        foreach (var o in Objects)
            if (o.isActive)
                o.Update();
        //update
        AsteroidsGame.Update();
        //
    }
    private void OnGUI()
    {
        foreach (var o in UI_Objects)
            if (o.isActive)
                o.OnGUI();
    }

    public new static List<T> FindObjectsOfType<T>()
    {
        List<T> result = new List<T>();
        foreach (var l in Objects)
        {
            var t = l.GetPart<T>();
            if (t != null)
                result.Add(t);
        }
        foreach (var l in addq)
        {
            var t = l.GetPart<T>();
            if (t != null)
                result.Add(t);
        }
        return result;
    }
    public new static Coroutine StartCoroutine(IEnumerator routine)
    {
        return (current as MonoBehaviour).StartCoroutine(routine);
    }

    private void PushObjects()
    {
        for (int k = 0; k < addq.Count; k++)
        {
            var t = addq.Dequeue();
            if (t != null)
            {
                Objects.Add(t);
                t.Start();
            }
        }
        for (int k = 0; k < delq.Count; k++)
        {
            var t = delq.Dequeue();
            if (t != null)
            {
                Objects.Remove(t);
                t.Dispose();
            }
        }
    }
}