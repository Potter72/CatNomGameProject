using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace ProjectCatRoll.Events
{
    public static class EventManager
    {
        public static event UnityAction<int> OnItemSpawnRequest;
        public static event EventHandler<CatGodSizeEventArgs> OnCatGodSizeChanged;
        public static event UnityAction OnNextLevelState;
        public static event EventHandler OnCutscenePlay;
        public static event EventHandler OnCutsceneStop;

        public static void RequestItemSpawn(int numberOfItems)
        {
            OnItemSpawnRequest?.Invoke(numberOfItems);
        }

        public static void SendCatGodSizeChange(float size)
        {
            OnCatGodSizeChanged?.Invoke(null, new CatGodSizeEventArgs(size));
        }

        public static void NextLevelState()
        {
            OnNextLevelState?.Invoke();
        }

        public static void SendCutscenePlayedEvent()
        {
            if (OnCutscenePlay == null) Debug.LogError("oncutsceneplay has no subscribers");
            OnCutsceneStop?.Invoke(null, EventArgs.Empty);
        }

        public static void SendCutsceneStopEvent()
        {
            OnCutsceneStop?.Invoke(null, EventArgs.Empty);
        }
    }
}
