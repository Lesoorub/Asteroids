using System.Collections.Generic;
using UnityEngine;

public static class Chache
{
    public static Camera cam = Camera.main;
    public static Material stdmaterial = new Material(Shader.Find("Sprites/Default"));
    public static Material spritematerial = new Material(Shader.Find("Hidden/Sprite"));
    public static GUIStyle style;

    public static class Resources
    {
        public static Dictionary<string, Sprite> loaded = new Dictionary<string, Sprite>();
    }
}