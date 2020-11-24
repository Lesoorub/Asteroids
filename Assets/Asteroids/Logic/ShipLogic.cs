using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ShipLogic : Part
{
    public Vector2Int LasersCharges = new Vector2Int(3, 3);//.x-current, .y-maximum
    public Timer LaserCooldown = new Timer(3);//every 3 second
    public Timer ShotCooldown = new Timer(0.1f);
    public bool ShotReady = true;
    string[] tagsblacklist = { "Ship", "Untagged" };

    public ShipLogic()
    {
        LaserCooldown.OnTick += () =>
        {
            if (LasersCharges.x < LasersCharges.y)
            {
                LasersCharges.x++;
                AsteroidsGame.current.UpdateLaserCounter();
            }
        };
        ShotCooldown.OnTick += () =>
        {
            ShotReady = true;
        };
    }

    public override void Update()
    {
        ShotCooldown.Update(Time.deltaTime * FastPhysics.TimeScale);
        LaserCooldown.Update(Time.deltaTime * FastPhysics.TimeScale);
        FastPhysics.CheckCollisions(FastPhysics, tagsblacklist, (item) =>
        {
            //TODO audio
            AsteroidsGenerator.Crush(item.logicobj);
            OnDamage();
        });
    }

    void OnDamage()
    {
        AsteroidsGame.current.OnGameOver();
    }

    public void Shot()
    {
        if (!ShotReady) return;
        ShotReady = false;
        float BulletSpeed = 5f;
        //
        var up = logicobj.up;
        var obj = Prefabs.Bullet();
        obj.position = position + up / 2f;
        obj.angle = logicobj.angle;
        obj.SpriteDrawer.PartActive = !Settings.PolyMode;
        obj.PolyRenderer.PartActive = Settings.PolyMode;
        obj.FastPhysics.Velocity = up * BulletSpeed + FastPhysics.Velocity.magnitude * up.normalized;
    }

    public void TryLaserShot()
    {
        float LaserLen = 10f;
        if (LasersCharges.x > 0)
        {
            LasersCharges.x--;
            AsteroidsGame.current.UpdateLaserCounter();
            var up = logicobj.up;
            foreach (var item in Engine.FindObjectsOfType<FastPhysics>())
                if (!FastPhysics.CrossArray(item.logicobj.tags, tagsblacklist))
                {
                    //Расстояние до лазера от объекта это длина его перпендикуляра если корабль смотрит в сторону объекта
                    var ip = item.position;
                    if (Vector2.Distance(up + position, ip) < Vector2.Distance(position, ip))//Если корабль смотрит в сторону объекта
                    {
                        Vector2 cross = PerpPoint(ip - position, up);
                        if (cross.magnitude < item.Radius && Vector2.Distance(ip - cross, position) < LaserLen)//Длина перепендикуляра
                            AsteroidsGenerator.Crush(item.logicobj, true);
                    }
                }
            var laser = LogicalObject.Create()
                .AddPart(new PolyRenderer(new Vector2[] {
                    new Vector2(0, 0.5f),
                    new Vector2(0, LaserLen)
                }, Color.red));
            laser.position = position;
            laser.angle = angle;
            Destroy(laser, 0.1f);
        }
    }

    Vector2 PerpPoint(Vector2 M, Vector2 V)
    {
        float t = V.x * V.x + V.y * V.y;
        return new Vector2(
            (V.y * (V.y * M.x - V.x * M.y)) / t,
            (V.x * (-V.y * M.x + V.x * M.y)) / t);
    }
}
