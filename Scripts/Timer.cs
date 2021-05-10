using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject m_ball;

    [HideInInspector] public static float m_lastTime = 0f;

    private Transform m_trans;

    private Vector3 m_initSpawn;
    private Vector3 m_initScale;
    private Vector3 m_initPos;

    void Start()
    {
        m_trans = GetComponent<Transform>();
        m_initSpawn = m_ball.transform.position;
        m_initScale = m_trans.localScale;
        m_initPos = m_trans.position;
        m_lastTime = 0f;
    }

    void Update()
    {
        m_lastTime += Time.deltaTime;

        m_trans.localScale -= new Vector3(.01f * Time.deltaTime, 0f, 0f);
        m_trans.position -= new Vector3(.05f * Time.deltaTime, 0f, 0f);

        if (m_trans.localScale.x < 0f || Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }

    void ResetLevel()
    {
        m_trans.localScale = m_initScale;
        m_trans.position = m_initPos;
        m_ball.transform.position = m_initSpawn;
        m_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        m_ball.GetComponent<BallMovement>().isClicked = false;
        m_ball.GetComponent<BallMovement>().DestroyFocus();

        m_lastTime = 0f;
        BallMovement.m_lastShoots = 0;
    }
}
