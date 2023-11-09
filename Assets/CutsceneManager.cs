using System.Collections.Generic;
using UnityEngine.InputSystem;
using ProjectCatRoll.Events;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    [SerializeField] bool _playOnStart = false;
    [SerializeField] PlayableDirector[] _directors;

    private Queue<PlayableDirector> _directorQueue;

    public UnityEvent OnCutsceneStart;
    public UnityEvent OnCutsceneEnd;

    private PlayableDirector _currentDirector;
    public PlayableDirector CurrentDirector => _currentDirector;
    //each playabledirector is a cutscene.
    //if cutscenes are to be triggered in order you might as well use a queue or something similar
    


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //initialize queue

        _directorQueue = new Queue<PlayableDirector>();

        foreach (var director in _directors)
        {
            _directorQueue.Enqueue(director);
        }

        //hook into events
        EventManager.OnCatGodSizeChanged += OnCatGodSizeChanged;

        // Keyboard.current.onTextInput += (char c) => { if (c == 'p') PlayNext(); };
        //instead of hooking into the event oncatgodsizechange maybe rework the connected events to not be so granular and only send events when progression is meant to happen.
        //that way i dont have to do a bunch of if statements when responding to the method

    }

    private void Start()
    {

        if (_playOnStart)
        {
            PlayNext();
        }
    }

    int _recievedCatGodSizeEvents = 0;
    private void OnCatGodSizeChanged(object sender, CatGodSizeEventArgs e)
    {
        _recievedCatGodSizeEvents++;
        if (_recievedCatGodSizeEvents % 3 == 0)
        {
            PlayNext();
        }
    }

    private void OnCutscenePlayed(PlayableDirector obj)
    {
        OnCutsceneStart?.Invoke();
        EventManager.SendCutscenePlayedEvent();
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        OnCutsceneEnd?.Invoke();
        EventManager.SendCutsceneStopEvent();
    }

    public void PlayNext()
    {
        //null check
        if (_directorQueue == null) return;

        if (_currentDirector != null)
        {
            _currentDirector.stopped -= OnCutsceneStopped;
            _currentDirector.stopped -= x => _currentDirector.gameObject.SetActive(false);
            _currentDirector.played -= OnCutscenePlayed;
        }

        if (_directorQueue.Count > 0)
        {
            _currentDirector = _directorQueue.Dequeue();
        }


        _currentDirector.stopped += OnCutsceneStopped;
        _currentDirector.stopped += x => _currentDirector.gameObject.SetActive(false);
        _currentDirector.played += OnCutscenePlayed;
        _currentDirector.gameObject.SetActive(true);

        _currentDirector.Play();
    }
}
