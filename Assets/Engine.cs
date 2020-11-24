#region usings
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion usings

public static class Engine
{
    public static Queue<LogicalObject> isnew = new Queue<LogicalObject>();
    public static ConcurrentBag<LogicalObject> Objects = new ConcurrentBag<LogicalObject>();
    public static ConcurrentBag<UIObject> UI_Objects = new ConcurrentBag<UIObject>();
    public static IGameTemplate activegame;
    public static MonoBehaviour launcher;
    public static void Start(MonoBehaviour launcher, IGameTemplate game)
    {
        Engine.launcher = launcher;
        activegame = game;

        activegame.Start();

        foreach (var o in Objects)
            o.Start();
    }
    public static void MyPostRenderer(Camera cam)
    {
        for (int k = 0; k < isnew.Count; k++)
        {
            var obj = isnew.Dequeue();
            obj.Start();
            Objects.Add(obj);
        }

        foreach (var o in Objects)
            if (o.isActive)
                o.Update();

        activegame.Update();
    }
    public static void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKey(key))
            {
                foreach (var o in Objects)
                    if (o.isActive)
                        o.KeyInput(key);
                foreach (var o in UI_Objects)
                    if (o.isActive)
                        o.KeyInput(key);
            }
    }
    public static void OnGUI()
    {
        foreach (var o in UI_Objects)
            if (o.isActive)
                o.OnGUI();
    }
    public static void StartCoroutine(IEnumerator coruntine)
    {
        launcher.StartCoroutine(coruntine);
    }
    public static List<T> FindObjectsOfType<T>()
    {
        List<T> result = new List<T>();
        foreach (var l in Objects)
        {
            var t = l.GetPart<T>();
            if (t != null)
                result.Add(t);
        }
        return result;
    }
}