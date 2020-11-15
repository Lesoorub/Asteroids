using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public static class PrefabMaster
{
    static Dictionary<string, object> Prefablist = new Dictionary<string, object>();
    static Dictionary<string, object> Chache = new Dictionary<string, object>();
    //Methods

    public static void SaveToChache(string key, object obj)
    {
        if (Chache.ContainsKey(key))
            Chache[key] = obj;
        else
            Chache.Add(key, obj);
    }

    public static object LoadFromChache(string key)
    {
        if (!Chache.ContainsKey(key)) return null;
        else return Chache[key];
    }

    public static T Load<T>(string prefab)
    {
        if (!Prefablist.ContainsKey(prefab))
            Prefablist[prefab] = Resources.Load(prefab);
        return (T)Prefablist[prefab];
    }
    public static GameObject LoadPrefab(string prefab)
    {
        return Load<GameObject>(prefab);
    }
    public static void WarmupLoad(string prefab)
    {
        if (!Prefablist.ContainsKey(prefab))
            Prefablist[prefab] = Resources.Load(prefab);
    }
    public static IEnumerator WarmupLoadAll()
    {
        string[] paths = AssetDatabase.GetAllAssetPaths();
        List<string> list = new List<string>();
        for (int k = 0; k < paths.Length; k++)
        {
            string path = paths[k];
            if (path.StartsWith("Assets/Resources"))
            {
                if (path.Contains("."))
                {
                    list.Add(path.Substring(0, path.LastIndexOf('.')));
                }
            }
        }
        for (int k = 0; k < list.Count; k++)
        {
            string p = list[k];
            if (!Prefablist.ContainsKey(p))
                Prefablist[p] = Resources.Load(p);
            yield return null;
        }
    }
}