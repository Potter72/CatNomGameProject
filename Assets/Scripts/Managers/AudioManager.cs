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

    [SerializeField]
    private AudioSource _effectSource;

    [SerializeField]
    private AudioSource _musicSource;


    //const for magic strings
    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_AMBIANCE = "AmbianceVolume";
    const string MIXER_UI = "UIVolume";
    const string MIXER_EFFECTS = "EffectsVolume";

    public float MasterVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float AmbianceVolume { get; private set; }
    public float UIVolume { get; private set; }
    public float EffectsVolume { get; private set; }



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

        ChangeMasterVolume(48f);
    }



    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.PlayOneShot(clip);
    }


    //volume controls
    public void ChangeMasterVolume(float volume)
    {
        _mixer.SetFloat(MIXER_MASTER, Mathf.Log10(volume) * 20);
        MasterVolume = volume;
    }
    public void ChangeMusicVolume(float volume)
    {
        _mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(volume) * 20);
        MusicVolume = volume;
    }
    public void ChangeAmbianceVolume(float volume)
    {
        _mixer.SetFloat(MIXER_AMBIANCE, Mathf.Log10(volume) * 20);
        AmbianceVolume = volume;
    }
    public void ChangeUIVolume(float volume)
    {
        _mixer.SetFloat(MIXER_UI, Mathf.Log10(volume) * 20);
        UIVolume = volume;
    }
    public void ChangeEffectsVolume(float volume)
    {
        _mixer.SetFloat(MIXER_EFFECTS, Mathf.Log10(volume) * 20);
        EffectsVolume = volume;
    }

}