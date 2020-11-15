using System;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : Part
{
    public delegate void ClickHandler();
    public event ClickHandler onClick;
    private List<ClickHandler> handlers = new List<ClickHandler>();
    public UIButton(ClickHandler onclick)
    {
        handlers.Add(onclick);
        onClick += onclick;
    }
    public override void Dispose()
    {
        foreach (var h in handlers)
            onClick -= h;
    }
    public override void BtnUpdate(KeyCode key)
    {
        if (key == KeyCode.Mouse0)
        {
            Vector2 mpos = Input.mousePosition;
            if (mpos.x > position.x &&
                Screen.height - mpos.y > position.y &&
                mpos.x < position.x + scale.x &&
                Screen.height - mpos.y < position.y + scale.y)
                onClick?.Invoke();
        }
    }
}