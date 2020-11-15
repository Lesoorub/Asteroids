using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIText : Part
{
    public string Text = "";
    GUIStyle style;
    public UIText(GUIStyle style, string text)
    {
        this.style = style;
        Text = text;
    }
    public override void OnGUI()
    {
        GUI.Label(new Rect(position, scale), Text, style);
    }
}
