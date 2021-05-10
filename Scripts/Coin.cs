using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject m_ball;

    [HideInInspector] public static int m_lastLevel = 2;

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
            m_lastLevel++;
            Wind.DestroyAll();
            SceneManager.LoadScene(1);
        }
    }
}
