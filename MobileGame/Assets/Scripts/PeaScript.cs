using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaScript : MonoBehaviour
{
    public float MoveSpeed;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ZombieScript>())
        {
            collision.gameObject.GetComponent<ZombieScript>().NotifyMyLane();
            collision.gameObject.SetActive(false);
        }
    }
}
