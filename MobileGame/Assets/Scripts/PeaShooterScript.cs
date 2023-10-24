using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PeaShooterScript : GenericPlant
{
    public float PeaSpawnTimer;
    private float PeaSpawnInterval;

    public GameObject Pea;

    protected override void Update()
    {
        base.Update();
    }

    protected override void PlantFunctionality()
    {
        if (LaneManager.Instance.MonstersInLane[AssociatedTile.Y - 1].Count > 0)
        {
            if (LaneManager.Instance.MonstersInLane[AssociatedTile.Y - 1].Where(s => s.transform.position.x > transform.position.x).Any())
            {
                PeaSpawnInterval += Time.deltaTime;

                if (PeaSpawnInterval > PeaSpawnTimer)
                {
                    PeaSpawnInterval = 0;
                    GameObject thePea = Instantiate(Pea, transform.position, Quaternion.identity);
                }

            }

        }

    }
}
