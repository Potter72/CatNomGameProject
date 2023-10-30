using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace ProjectCatRoll.Events
{
    public static class EventManager
    {
        public static event UnityAction<int> OnItemSpawnRequest;
        public static event EventHandler<CatGodSizeEventArgs> OnCatGodSizeChanged;

        public static void RequestItemSpawn(int numberOfItems)
        {
            OnItemSpawnRequest?.Invoke(numberOfItems);
        }

        public static void SendCatGodSizeChange(float size)
        {
            OnCatGodSizeChanged?.Invoke(null, new CatGodSizeEventArgs(size));
        }
    }
}
