using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMainGameSound : MonoBehaviour
{
    [SerializeField] AudioClip inGameMusic;
    void Start()
    {
        AudioManager.Instance.PlayMusic(inGameMusic); 
    }
}