using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class AsteroidsGenerator
{
    public static int Limit = 10;
    public static List<LogicalObject> Objects = new List<LogicalObject>(10);
    public static Timer Spawner = new Timer(0.5f);
    public static float AsteroidsSpeed = 2;

    public static void Start()
    {
        Spawner.OnTick += () =>
        {
            if (Objects.Count < Limit)
            {
                if (UnityEngine.Random.Range(0, 100) < UFOLogic.SpawnChance)
                    CreateUFO(GetRandomPosition());
                else
                {
                    var obj = CreateAsteroid(GetRandomPosition());
                    var fp = obj.GetPart<FastPhysics>();
                    fp.Velocity =
                    (new Vector2(
                        UnityEngine.Random.Range(-AsteroidsGame.HalfScreenSize.x - 2, AsteroidsGame.HalfScreenSize.y - 2),
                        UnityEngine.Random.Range(-AsteroidsGame.HalfScreenSize.x - 2, AsteroidsGame.HalfScreenSize.y - 2)) - obj.position)
                    .normalized * AsteroidsSpeed;
                    fp.Angularvelocity = UnityEngine.Random.Range(-360f, 360f);
                }
            }
            else
            {
                while (true)
                {
                    if (Objects.Count > 0)
                    {
                        for (int k = 0; k < Objects.Count - 1; k++)
                        {
                            if (Objects[k] == null)
                            {
                                var t = Objects[k];
                                Objects[k] = Objects[k + 1];
                                Objects[k + 1] = t;
                            }
                        }
                        if (Objects[Objects.Count - 1] == null)
                            Objects.RemoveAt(Objects.Count - 1);
                        else
                            break;
                    }
                    else break;
                }
            }
        };
    }
    public static void Update()
    {
        Spawner.Update(Time.deltaTime * FastPhysics.TimeScale);
    }
    //static
    //objs
    public static LogicalObject CreateAsteroid(Vector2 pos)
    {
        var obj = Prefabs.Asteroid();
        obj.position = pos;
        RandomShape(obj.GetPart<PolyRenderer>());
        obj.GetPart<SpriteDrawer>().PartActive = !Settings.PolyMode;
        obj.GetPart<PolyRenderer>().PartActive = Settings.PolyMode;
        Objects.Add(obj);
        return obj;
    }
    public static LogicalObject CreateMeteor(Vector2 pos)
    {
        var obj = Prefabs.Meteor();
        obj.position = pos;
        RandomShape(obj.GetPart<PolyRenderer>());
        obj.GetPart<SpriteDrawer>().PartActive = !Settings.PolyMode;
        obj.GetPart<PolyRenderer>().PartActive = Settings.PolyMode;
        return obj;
    }
    public static LogicalObject CreateUFO(Vector2 pos)
    {
        var obj = Prefabs.UFO();
        obj.position = pos;
        obj.GetPart<SpriteDrawer>().PartActive = !Settings.PolyMode;
        obj.GetPart<PolyRenderer>().PartActive = Settings.PolyMode;
        Objects.Add(obj);
        return obj;
    }
    //Events
    public static void Crush(LogicalObject @object, bool isLaser = false)
    {
        var t = @object.GetParts<ICrush>();
        ;
        foreach (var c in t)
            c.OnCrush(isLaser);
    }
    //tools
    static void RandomShape(PolyRenderer pr, float radius = 1, int max = 10)
    {
        Vector2[] points = new Vector2[max];
        for (int k = 0; k < max; k++)
        {
            float angle = ((float)k / (float)max) * 360f;
            points[k] = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) 
                * (radius * UnityEngine.Random.Range(0.6f, 1f) / 2f);
        }
        pr.Points = points;
    }
    public static Vector2 GetRandomPosition()
    {
        float distance = AsteroidsGame.HalfScreenSize.magnitude;
        //
        float angle = UnityEngine.Random.Range(0, 360);
        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) *
            (distance + UnityEngine.Random.Range(0, 2f));
    }
}