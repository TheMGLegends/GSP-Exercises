using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public int P1_Score;
    public int P2_Score;

    private GameObject ball;
    public float respawnTimer;
    public float delay = 1;

    public TMP_Text P1ScoreText;
    public TMP_Text P2ScoreText;

    private void Awake()
    {
        gameManager = this;
    }

    void Start()
    {
        UpdateUI();
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

    public void UpdateUI()
    {
        P1ScoreText.text = P1_Score.ToString();
        P2ScoreText.text = P2_Score.ToString();
    }
}
