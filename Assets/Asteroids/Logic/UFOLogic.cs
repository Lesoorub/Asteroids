using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UFOLogic : Part, ICrush
{
    public static float SpawnChance = 10;
    public float UfoSpeed = 3;
    public int ScoreReward = 100;
    FastPhysics fp;
    public override void Start()
    {
        fp = GetPart<FastPhysics>();
    }
    public override void Update()
    {
        if (AsteroidsGame.ship != null)
            fp.Velocity = (AsteroidsGame.ship.position - position).normalized * UfoSpeed;
    }
    public void OnCrush(bool islaser = false)
    {
        AsteroidsGenerator.Objects.Remove(logicobj);
        Destroy();
        AsteroidsGame.Score += ScoreReward;
    }
}