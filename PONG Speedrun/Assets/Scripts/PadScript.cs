using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PadScript : MonoBehaviour
{
    public float VerticalSpeed;
    private Rigidbody2D rb;

    public enum PlayerID
    {
        P1,
        P2
    }

    public PlayerID player;
    private string PlayerString;

    public float yLimit;

    private void Start()
    {       
        rb = GetComponent<Rigidbody2D>();

        switch (player)
        {
            case PlayerID.P1:
                PlayerString = "_P1";
                break;
            case PlayerID.P2:
                PlayerString = "_P2";
                break;
        }
    }

    private void Update()
    {
        rb.velocity = new Vector2(0, Input.GetAxisRaw("Vertical" + PlayerString) * VerticalSpeed);
    }

    private void LateUpdate()
    {
        if (transform.position.y > yLimit)
        {
            transform.position = new Vector3(transform.position.x, yLimit, transform.position.z);
        }
        
        if (transform.position.y < -yLimit)
        {
            transform.position = new Vector3(transform.position.x, -yLimit, transform.position.z);
        }
    }
}
