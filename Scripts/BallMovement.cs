using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_target;

    [HideInInspector] public static int m_lastShoots = 0;

    private Rigidbody m_body;
    private Transform m_trans;

    private float m_x = 0;
    private float m_y = 0;

    [HideInInspector] public bool isClicked = false;

    private GameObject m_a;
    private GameObject m_b;
    private GameObject m_c;

    void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_trans = GetComponent<Transform>();
        m_lastShoots = 0;
    }

    void Update()
    {
        if (Physics.Raycast(m_trans.position + new Vector3(0.125f, 0f), Vector3.down, 0.125f) || Physics.Raycast(m_trans.position - new Vector3(0.125f, 0f), Vector3.down, 0.125f))
        {
            if (m_body.velocity.x > 0.1f)
            {               
                isClicked = false;
                DestroyFocus();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isClicked = true;
                m_x = Input.mousePosition.x;
                m_y = Input.mousePosition.y;

                CreateFocus();
            }        
            else if (Input.GetKeyUp(KeyCode.Mouse0) && isClicked == true)
            {
                isClicked = false;
                m_lastShoots++;

                DestroyFocus();

                float x = Limit((m_x - Input.mousePosition.x), 150f);
                float y = Limit((m_y - Input.mousePosition.y), 200f);

                m_body.AddForce(x, y, 0);
            }

            if (isClicked == true)
            {
                ModifFocus();
            }

            m_body.velocity = Vector3.zero;
            m_body.angularVelocity = Vector3.zero;
        }
    }

    float Limit(float a, float lim)
    {
        if (a > lim)
            a = lim;
        else if (a < -lim)
            a = -lim;
        return a;
    }

    void CreateFocus()
    {
        if (m_a != null)
        {
            DestroyFocus();
            isClicked = false;
            return;
        }

        float x = Limit((m_x - Input.mousePosition.x), 15f) + m_trans.position.x;
        float y = Limit((m_y - Input.mousePosition.y), 20f) + m_trans.position.y;

        Vector3 vec1 = new Vector3(x * .33f, y * .33f, 2f);
        Vector3 vec2 = new Vector3(x * .66f, y * .66f, 2f);
        Vector3 vec3 = new Vector3(x, y, 2f);

        m_a = GameObject.Instantiate(m_target, vec1, m_trans.rotation);
        m_b = GameObject.Instantiate(m_target, vec2, m_trans.rotation);
        m_c = GameObject.Instantiate(m_target, vec3, m_trans.rotation);
    }

    void ModifFocus()
    {
        float x = Limit((m_x - Input.mousePosition.x), 150f);
        float y = Limit((m_y - Input.mousePosition.y), 200f);

        m_a.transform.position = new Vector3(x * .33f, y * .33f, 2f) * .01f + m_trans.position;
        m_b.transform.position = new Vector3(x * .66f, y * .66f, 2f) * .01f + m_trans.position;
        m_c.transform.position = new Vector3(x, y, 2f) * .01f + m_trans.position;

        float scale = (Math.Abs(x) + Math.Abs(y)) * 0.001f;
        m_a.transform.localScale = new Vector3(scale * .33f, scale * .33f, scale * .33f);
        m_b.transform.localScale = new Vector3(scale * .66f, scale * .66f, scale * .66f);
        m_c.transform.localScale = new Vector3(scale, scale, scale);
    }

    [HideInInspector] public void DestroyFocus()
    {
        Destroy(m_a);
        Destroy(m_b);
        Destroy(m_c);
    }
}
