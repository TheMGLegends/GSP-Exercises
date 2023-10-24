using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlowerScript : GenericPlant
{
    public float SunSpawnTimer;
    private float SunSpawnInterval;

    public GameObject Sun;

    protected override void Update()
    {
        base.Update();
    }

    protected override void PlantFunctionality()
    {
        SunSpawnInterval += Time.deltaTime;
        if (SunSpawnInterval > SunSpawnTimer)
        {
            SunSpawnInterval = 0;

            GameObject theSun = Instantiate(Sun, transform.position, Quaternion.identity);
        }
    }
}
