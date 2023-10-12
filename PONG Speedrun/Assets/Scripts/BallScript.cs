using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public enum InitialDirection
    {
        UpRight,
        Upleft,
        DownRight,
        DownLeft
    }

    public InitialDirection direction;
    public float ballSpeed;

    private Rigidbody2D rb;

    private Vector3 originLocation;

    private void Start()
    {
        originLocation = transform.position;
        rb = GetComponent<Rigidbody2D>();

        Respawn();
    }

    internal void Respawn()
    {
        transform.position = originLocation;

        int rand = UnityEngine.Random.Range(0, 4); // 0, 1, 2, 3
        direction = (InitialDirection)rand;

        StartCoroutine(MovementDelayCoroutine());

    }

    private IEnumerator MovementDelayCoroutine()
    {
        yield return new WaitForSeconds(GameManager.gameManager.delay);

        switch (direction)
        {
            case InitialDirection.UpRight:
                rb.velocity = new Vector2(1, 1) * ballSpeed;
                break;
            case InitialDirection.Upleft:
                rb.velocity = new Vector2(-1, 1) * ballSpeed;
                break;
            case InitialDirection.DownRight:
                rb.velocity = new Vector2(1, -1) * ballSpeed;
                break;
            case InitialDirection.DownLeft:
                rb.velocity = new Vector2(-1, -1) * ballSpeed;
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
        else if (collision.CompareTag("Pad"))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
    }
}
