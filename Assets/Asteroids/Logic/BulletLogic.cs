using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BulletLogic : Part
{
    float lifetime = 3;//Destroy after 3 seconds
    string[] tagsblacklist = { "Ship", "Untagged" };
    public override void Update()
    {
        FastPhysics.CheckCollisions(FastPhysics, tagsblacklist, (item) => {
            //TODO audio
            AsteroidsGenerator.Crush(item.logicobj);
            Destroy();
        });
        lifetime -= Time.deltaTime * FastPhysics.TimeScale;
        if (lifetime <= 0)
            Destroy();
    }
}