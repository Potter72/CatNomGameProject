using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Animator _fader;
    
    private void Awake()
    {
        Debug.Log("Yes");
        _fader.SetTrigger("Fade");
    }
    
}
