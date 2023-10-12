using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFieldScript : MonoBehaviour
{
    public enum PlayerScoreField
    {
        P1,
        P2
    }

    public PlayerScoreField scoreField;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            switch (scoreField)
            {
                case PlayerScoreField.P1:
                    ++GameManager.gameManager.P1_Score;
                    break;
                case PlayerScoreField.P2:
                    ++GameManager.gameManager.P2_Score;
                    break;
            }
        }
        GameManager.gameManager.DespawnBall();
    }
}
