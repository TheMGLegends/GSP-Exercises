using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public int P1_Score;
    public int P2_Score;

    private GameObject ball;
    public float respawnTimer;
    public float delay;

    void Start()
    {
        gameManager = this;
        ball = GameObject.Find("Ball");
    }
    internal void DespawnBall()
    {
        ball.SetActive(false);
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTimer);

        ball.SetActive(true);
        ball.GetComponent<BallScript>().Respawn();
    }
}
