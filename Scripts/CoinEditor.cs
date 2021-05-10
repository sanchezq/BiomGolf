using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinEditor : MonoBehaviour
{
    [SerializeField] private GameObject m_editor;
    [SerializeField] private GameObject m_ball;
    [SerializeField] private Text m_textScore;

    private Transform m_trans;

    void Start()
    {
        m_trans = GetComponent<Transform>();
    }

    void Update()
    {
        m_trans.Rotate(Vector3.up, 100f * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_ball)
        {
            int time = (int)(Timer.m_lastTime * 10f);

            int score = 500 - time - BallMovement.m_lastShoots * 5;
            if (score < 0)
                score = 0;

            m_textScore.text = "Last Game" +
                          "\n\nTime: " + ((float)time) * 0.1 + "s\n" +
                          "Shots: " + BallMovement.m_lastShoots +
                          "\n\nFinal score: " + score;

            m_editor.GetComponent<Editor>().SwapEditor();
        }
    }
}
