using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

public static class Extended
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    public static bool Remove<T>(this ConcurrentBag<T> t, T item)
    {
        bool isFind = false;
        List<T> temp = new List<T>();
        lock (t)
        {
            foreach (var i in t)
                if (!i.Equals(item))
                {
                    temp.Add(i);
                    isFind = true;
                }
            if (isFind)
            {
                t.Clear();
                foreach (var i in temp)
                    t.Add(i);
            }
        }
        return isFind;
    }
    public static void Clear<T>(this ConcurrentBag<T> t)
    {
        lock(t)
            while (!t.IsEmpty)
                t.TryTake(out T _);
    }
    public static Vector2 TowardWithSpeed(this Vector2 from, Vector2 to, float speed) =>
        (to - from).normalized * speed;
}