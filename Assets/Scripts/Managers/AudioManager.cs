using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//summary:
//audiomanager for cat roll game

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mixer;


    private AudioSource _effectSource;
    private AudioSource _musicSource;


    //const for magic strings

    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_AMBIANCE = "AmbianceVolume";
    const string MIXER_UI = "UIVolume";
    const string MIXER_EFFECTS = "EffectsVolume";

    
    void Start()
    {
        //singleton declaration
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);      
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float volume)
    {
        _mixer.SetFloat(MIXER_MASTER, Mathf.Log10(volume) * 20);
    }
}
