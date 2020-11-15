using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Prefabs
{
    public static LogicalObject Ship()
    {
        return LogicalObject.Create(tag: "Ship")
        .AddPart(new PolyRenderer(new Vector2[]
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0, 0.5f),
        }))
        .AddPart(new SpriteDrawer(source: Chache.Resources.loaded["ship"]))
        .AddPart<ShipLogic>()
        .AddPart<ShipControll>()
        .AddPart(new FastPhysics(drag: 3, radius: 0.5f));
    }
    public static LogicalObject Bullet()
    {
        return LogicalObject.Create(Vector2.zero, new Vector2(0.2f, 0.2f), tag: "Untagged")
        .AddPart(new PolyRenderer(new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        }))
        .AddPart(new SpriteDrawer(Chache.Resources.loaded["bullet"]))
        .AddPart(new FastPhysics(drag: 0, radius: 0.05f));
    }
    public static LogicalObject Asteroid()
    {
        return LogicalObject.Create(tag: "Asteroid")
        .AddPart<PolyRenderer>()
        .AddPart(new SpriteDrawer(Chache.Resources.loaded["Asteroid" + UnityEngine.Random.Range(0, 3)]))
        .AddPart<AsteroidsLogic>()
        .AddPart(new FastPhysics(0, 0.5f));
    }
    public static LogicalObject Meteor()
    {
        return LogicalObject.Create(Vector2.zero, new Vector2(0.8f, 0.8f) ,tag: "Meteor")
        .AddPart<PolyRenderer>()
        .AddPart(new SpriteDrawer(Chache.Resources.loaded["Asteroid" + UnityEngine.Random.Range(0, 3)]))
        .AddPart<MeteorsLogic>()
        .AddPart(new FastPhysics(0, 0.4f));
    }
    public static LogicalObject UFO()
    {
        return LogicalObject.Create(tag: "UFO")
        .AddPart(new PolyRenderer(new Vector2[] {
            new Vector2(-0.5f, -0.1f),
            new Vector2(-0.3f, -0.3f),
            new Vector2(0.3f, -0.3f),
            new Vector2(0.5f, -0.1f),
            new Vector2(0.3f, 0.1f),
            new Vector2(-0.3f, 0.1f),
            new Vector2(-0.5f, -0.1f),
            new Vector2(0.5f, -0.1f),
            new Vector2(0.3f, 0.1f),
            new Vector2(0.3f, 0.1f),
            new Vector2(0.2f, 0.1f),
            new Vector2(0.1f, 0.3f),
            new Vector2(-0.1f, 0.3f),
            new Vector2(-0.2f, 0.1f),
            new Vector2(-0.3f, 0.1f),
        }))
        .AddPart(new SpriteDrawer(Chache.Resources.loaded["UFO"]))
        .AddPart<UFOLogic>()
        .AddPart(new FastPhysics(0, 0.5f));
    }
    public static LogicalObject Score()
    {
        float w = Screen.width,
              h = 40;
        return LogicalObject.UICreate(new Rect(5, 0, w, h))
        .AddPart(new UIText(Chache.style, "Score: 0"));
    }
    public static LogicalObject GameOver()
    {
        float w = 400,
              h = 40;
        return LogicalObject.UICreate(new Rect(Screen.width / 2 - w / 2, Screen.height / 2 - h / 2, w, h))
        .AddPart(new UIText(Chache.style, "GameOver"))
        .AddPart(new UIButton(AsteroidsGame.ApplyGameOver));
    }
    public static LogicalObject LaserCounter()
    {
        float w = Screen.width,
              h = 40;
        return LogicalObject.UICreate(new Rect(5, Screen.height - h, w, h))
        .AddPart(new UIText(Chache.style, "Lasers: 0"));
    }
    public static LogicalObject CheckBoxPolyMode()
    {
        float w = 250,
              h = 40;
        return LogicalObject.UICreate(new Rect(Screen.width - w, 0, w, h))
        .AddPart(new UIText(Chache.style, "PolyMode [ ]"))
        .AddPart(new UIButton(AsteroidsGame.ChangePolyMode));
    }
}