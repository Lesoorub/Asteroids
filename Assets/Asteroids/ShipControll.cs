using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShipControll : Part
{
    public float Speed = 50;
    ShipLogic sl;
    FastPhysics fp;
    public override void Start()
    {
        fp = logicobj.GetPart<FastPhysics>();
        sl = logicobj.GetPart<ShipLogic>();
    }
    public override void KeyInput()
    {
        //Look ship to mouse
        Vector2 target = Chache.cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        Vector2 moveDirection = new Vector2(target.x, target.y) - position;
        if (moveDirection != Vector2.zero && FastPhysics.TimeScale > 0)
            logicobj.angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;

        if (UnityEngine.Input.anyKey && FastPhysics.TimeScale != 0)
        {
            if (UnityEngine.Input.GetKey(KeyCode.W))
                fp.AddForce(logicobj.up * Time.deltaTime * Speed);
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))//Shot
                sl.Shot();
            if (UnityEngine.Input.GetKeyDown(KeyCode.S) || UnityEngine.Input.GetKeyDown(KeyCode.Mouse1))//Laser
                sl.TryLaserShot();
        }
    }
}