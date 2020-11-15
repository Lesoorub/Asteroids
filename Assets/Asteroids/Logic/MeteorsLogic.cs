using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorsLogic : Part, ICrush
{
    public int ScoreReward = 10;
    public void OnCrush(bool islaser = false)
    {
        Destroy();
        AsteroidsGame.Score += ScoreReward;
    }
}
