using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private string scene;

    public void SwitchScene ()
    {
        SceneManager.LoadScene(scene);
        Debug.Log("Changed Scene!");
    }

    public void QuitGame ()
    {
        Debug.Log ("Quit!");
        Application.Quit();
    }
}
