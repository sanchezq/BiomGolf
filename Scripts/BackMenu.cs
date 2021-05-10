using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Coin.m_lastLevel = 2;
            Wind.DestroyAll();
            SceneManager.LoadScene(0);
        }
    }
}
