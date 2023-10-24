using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public List<GameObject> ZombieSpawnNode = new List<GameObject>();

    public List<List<ZombieScript>> MonstersInLane = new List<List<ZombieScript>>();

    public GameObject Zombie;

    public float ZombieSpawnTimer;
    private float ZombieSpawnInterval;

    public static int ZombSpawned;

    public static LaneManager Instance;

    private void Awake()
    {
        Instance = this;

        MonstersInLane.Add(new List<ZombieScript>());
        MonstersInLane.Add(new List<ZombieScript>());
        MonstersInLane.Add(new List<ZombieScript>());
        MonstersInLane.Add(new List<ZombieScript>());
        MonstersInLane.Add(new List<ZombieScript>());

    }

    private void Update()
    {
        ZombieSpawnInterval += Time.deltaTime;

        if (ZombieSpawnInterval > ZombieSpawnTimer)
        {
            ZombieSpawnInterval = 0;

            int Rand = UnityEngine.Random.Range(0, 5);

            GameObject theZombie = Instantiate(Zombie, ZombieSpawnNode[Rand].transform.position, Quaternion.identity);

            Zombie.GetComponent<ZombieScript>().ZombieID = ZombSpawned++;
            Zombie.GetComponent<ZombieScript>().LaneID = Rand;
            MonstersInLane[Rand].Add(Zombie.GetComponent<ZombieScript>());
        }
    }

    internal void RemoveZombie(int zombieID, int laneID)
    {
        ZombieScript Zomb = MonstersInLane[laneID].Where(s=> s.ZombieID == zombieID).FirstOrDefault();
        MonstersInLane[laneID].Remove(Zomb);
    }
}
