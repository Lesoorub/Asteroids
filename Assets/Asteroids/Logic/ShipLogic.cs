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
    public List<LogicalObject> bullets = new List<LogicalObject>(50);

    public ShipLogic()
    {
        LaserCooldown.OnTick += () =>
        {
            if (LasersCharges.x < LasersCharges.y)
            {
                LasersCharges.x++;
                AsteroidsGame.UpdateLaserCounter();
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
        CalcCollisions(position, (item) =>
        {
            //TODO audio
            AsteroidsGenerator.Crush(item.logicobj);
            OnDamage();
        });
    }

    void OnDamage()
    {
        AsteroidsGame.OnGameOver();
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
        obj.GetPart<SpriteDrawer>().PartActive = !Settings.PolyMode;
        obj.GetPart<PolyRenderer>().PartActive = Settings.PolyMode;
        var fp = obj.GetPart<FastPhysics>();
        fp.Velocity = up * BulletSpeed + GetPart<FastPhysics>().Velocity.magnitude * up.normalized;
        Engine.StartCoroutine(BulletCalcCollisions(obj));
        bullets.Add(obj);
    }

    IEnumerator BulletCalcCollisions(LogicalObject bullet)
    {
        float lifetime = 3;//Destroy after 3 seconds
        while (bullet != null)
        {
            CalcCollisions(bullet.position, (item) => {
                //TODO audio
                AsteroidsGenerator.Crush(item.logicobj);
                bullets.Remove(bullet);
                LogicalObject.Destroy(bullet);
                bullet = null;
            });
            lifetime -= Time.deltaTime * FastPhysics.TimeScale;
            if (lifetime <= 0)
            {
                bullets.Remove(bullet);
                LogicalObject.Destroy(bullet);
                bullet = null;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void TryLaserShot()
    {
        float LaserLen = 10f;
        if (LasersCharges.x > 0)
        {
            LasersCharges.x--;
            AsteroidsGame.UpdateLaserCounter();
            var up = logicobj.up;
            foreach (var item in Engine.FindObjectsOfType<FastPhysics>())
                if (item.tag != "Ship" && item.tag != "Untagged")
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

    void CalcCollisions(Vector2 pos, UnityAction<FastPhysics> onCollision)
    {
        float r = logicobj.GetPart<FastPhysics>().Radius;
        foreach (var item in Engine.FindObjectsOfType<FastPhysics>())
        {
            if (item.tag != "Ship" && item.tag != "Untagged")
                if (Vector2.Distance(item.position, pos) < item.Radius + r)
                {
                    onCollision(item);
                    continue;
                }
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
