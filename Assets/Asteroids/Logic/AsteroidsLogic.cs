using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsLogic : Part, ICrush
{
    public float MeteorsSpeed = 4;
    public int PartsCount = 3;
    public int ScoreReward = 20;

    public void OnCrush(bool islaser = false)
    {
        if (!islaser)
        {
            float offset = Random.Range(0, 360);
            for (int k = 0; k < PartsCount; k++)
            {
                float angle = (((float)k / (float)PartsCount) * 360f + offset) % 360;
                var obj = AsteroidsGenerator.CreateMeteor(position);
                var fp = obj.GetPart<FastPhysics>();
                fp.Velocity =
                    new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * MeteorsSpeed;
                fp.Angularvelocity = UnityEngine.Random.Range(-360f, 360f);
            }
        }
        Destroy();
        AsteroidsGenerator.Objects.Remove(logicobj);
        AsteroidsGame.Score += ScoreReward;
    }
}
