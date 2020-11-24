using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UFOLogic : Part, ICrush
{
    public static float SpawnChance = 10;
    public float UfoSpeed = 3;
    public int ScoreReward = 100;
    public override void Update()
    {
        if (AsteroidsGame.current.ship != null)
            FastPhysics.Velocity = position.TowardWithSpeed(AsteroidsGame.current.ship.position, UfoSpeed);
    }
    public void OnCrush(bool islaser = false)
    {
        AsteroidsGenerator.Objects.Remove(logicobj);
        Destroy();
        AsteroidsGame.current.Score += ScoreReward;
    }
}