using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static bool GameIsPaused = false;

    void Update ()
    {
        
    }

    public void ResumingGame ()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void PausingGame ()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
