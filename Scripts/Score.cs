using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    void Start()
    {
        int time = (int)(Timer.m_lastTime * 10f);

        int score = 500 - time - BallMovement.m_lastShoots * 5;
        if (score < 0)
            score = 0;

        Text m_text = GetComponent<Text>();
        m_text.text = "Level " + (Coin.m_lastLevel - 2) + " finished" +
                      "\n\nTime: " + ((float)time) * 0.1 + "s\n" +
                      "Shots: " + BallMovement.m_lastShoots +
                      "\n\nFinal score: " + score;
    }
}
