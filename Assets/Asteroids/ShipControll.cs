using System;
using UnityEngine;

public class ShipControll : Part
{
    public float Speed = 50;
    ShipLogic sl;
    public override void Start()
    {
        sl = logicobj.GetPart<ShipLogic>();
    }
    public override void Update()
    {
        //Look ship to mouse
        Vector2 target = Chache.cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 moveDirection = new Vector2(target.x, target.y) - position;
        if (moveDirection != Vector2.zero && FastPhysics.TimeScale > 0)
            logicobj.angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;

    }
    public override void KeyInput(KeyCode key)
    {
        if (Input.anyKey && FastPhysics.TimeScale != 0)
        {
            if (key == KeyCode.W)
                FastPhysics.AddForce(logicobj.up * Time.deltaTime * Speed);
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))//Shot
                sl.Shot();
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Mouse1))//Laser
                sl.TryLaserShot();
        }
    }
}