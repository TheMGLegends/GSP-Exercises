using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public int ZombieID;

    public float MoveSpeed;

    internal int LaneID;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, 0);
    }

    public void NotifyMyLane()
    {
        LaneManager.Instance.RemoveZombie(ZombieID, LaneID);
    }
}
