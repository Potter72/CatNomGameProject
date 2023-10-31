using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System;
using ProjectCatRoll.Events;

public class DoThingAtGodSize : MonoBehaviour
{
    [SerializeField] private float _goalSize;
    public UnityEvent OnGoalSizeReached;

    private void Awake()
    {
        EventManager.OnCatGodSizeChanged += CheckGodSize;
    }

    private void CheckGodSize(object sender, CatGodSizeEventArgs e)
    {
        if(e.Size >= _goalSize)
        {
            OnGoalSizeReached?.Invoke();
            EventManager.OnCatGodSizeChanged -= CheckGodSize;
        }
    }
}
