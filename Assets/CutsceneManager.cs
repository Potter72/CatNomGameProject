using System.Collections.Generic;
using UnityEngine.InputSystem;
using ProjectCatRoll.Events;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine;
using System;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }


    [SerializeField] private PlayableDirector[] _directors;

    private Queue<PlayableDirector> _directorQueue;

    private PlayableDirector _currentDirector;

    public PlayableDirector CurrentDirector => _currentDirector;
    //each playabledirector is a cutscene.
    //if cutscenes are to be triggered in order you might as well use a queue or something similar


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _directorQueue = new Queue<PlayableDirector>();

        foreach (var director in _directors)
        {
            _directorQueue.Enqueue(director);
        }
        
        //hook into events
        EventManager.OnCatGodSizeChanged += OnCatGodSizeChanged;

        Keyboard.current.onTextInput += (char c) => { if (c == 'p') PlayNext(); };
        //instead of hooking into the event oncatgodsizechange maybe rework the connected events to not be so granular and only send events when progression is meant to happen.
        //that way i dont have to do a bunch of if statements when responding to the method

    }

    int _recievedCatGodSizeEvents = 0;
    private void OnCatGodSizeChanged(object sender, CatGodSizeEventArgs e)
    {
        _recievedCatGodSizeEvents++;
        if(_recievedCatGodSizeEvents % 3 == 0)
        {
            PlayNext();
        }
    }

    private void OnCutscenePlayed(PlayableDirector obj)
    {
        EventManager.SendCutscenePlayedEvent();
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        EventManager.SendCutsceneStopEvent();
    }


    void Start()
    {

    }

    public void PlayNext()
    {
        //null check
        if(_directorQueue == null) return;

        if(_currentDirector != null)
        {
            _currentDirector.stopped -= OnCutsceneStopped;
            _currentDirector.played -= OnCutscenePlayed;
        }
        if (_directorQueue.Count > 0)
        {
            _currentDirector = _directorQueue.Dequeue();
        }
        _currentDirector.stopped += OnCutsceneStopped;
        _currentDirector.played += OnCutscenePlayed;

        _currentDirector.Play();
    }
}
