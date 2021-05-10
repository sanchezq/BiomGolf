using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void GoMenu()
    {
        Coin.m_lastLevel = 2;
        SceneManager.LoadScene(0);
    }

    public void GoLevelEditor()
    {
        SceneManager.LoadScene(4);
    }

    public void GoNextLevel()
    {
        SceneManager.LoadScene(Coin.m_lastLevel);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
