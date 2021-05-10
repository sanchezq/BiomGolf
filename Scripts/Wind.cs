using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private GameObject m_ball;
    [SerializeField] private GameObject m_sand;

    private Transform m_trans;
    private Rigidbody m_body;

    private float m_time = 0f;

    private System.Random m_random = new System.Random();

    private static List<GameObject> m_list = new List<GameObject>();

    void Start()
    {
        m_trans = GetComponent<Transform>();
        m_body = m_ball.GetComponent<Rigidbody>();
        m_time = 0f;
    }

    void Update()
    {
        if (m_trans.position.z != 250f)
            return;

        m_body.AddForce(new Vector3(Time.deltaTime * 100f, 0f));

        m_time += Time.deltaTime;

        if (m_time > .2f)
        {
            m_list.Add(Instantiate(m_sand, new Vector3(-12f, m_random.Next(10), -2f), m_sand.transform.rotation));
            m_list[m_list.Count - 1].transform.localScale = new Vector3(.1f, .1f, .1f);
            m_time = 0f;
        }

        foreach (GameObject g in m_list)
        {
            g.transform.position += Vector3.right * Time.deltaTime * 10f;
            g.transform.Rotate(Vector3.forward * -1f, 500f * Time.deltaTime, Space.Self); ;
        }

        Destroy();
    }

    private void Destroy()
    {
        int i = 0;
        foreach (GameObject g in m_list)
        {
            if (g.transform.position.x > 12f)
            {
                Destroy(g);
                m_list.RemoveAt(i);
                Destroy();
                return;
            }
            i++;
        }
    }
    [HideInInspector] public static void DestroyAll()
    {
        foreach (GameObject g in m_list)
        {
            Destroy(g);
        }
        m_list.Clear();
    }
}
